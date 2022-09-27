using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���¸� ������ ������ Ÿ��
public enum GhostState
{
    None,
    Depart,  // ���
    Walk   // ���ƴٴϱ�
}

public class GhostAI : MonoBehaviour
{
    public GhostState State;
    public GhostState PrevState = GhostState.None;
    public Direction NowDirection;

    private const float ERROR_RANGE = 0.01f;

    private bool _directionToggle;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private Vector3 _destination;

    private Vector3 _startPosition;
    private bool _isDepart = false;

    private int _walkCount = 0;
    private int _randomNum = 1;

    private void Awake()
    {
        State = GhostState.Depart;
    }

    private void Update()
    {
        if(!GameManager.Instance.IsPause)
        {
            // ������ �����Ǿ��� ���� �̵�
            if (_directionToggle)
            {
                StartCoroutine(MoveSmoothly());
                return;
            }

            // ���°� ����� �� �� ���� ����
            if (State != PrevState)
            {
                switch (State)
                {
                    case GhostState.Depart: StartCoroutine(Depart()); break;
                    case GhostState.Walk: StartCoroutine(Walk()); break;
                    default: break;
                }
            }
        }
    }

    // ������ �÷��̾ ����� ���, ������ ����� ���� ��ǥ�� �����ϰ�
    // Depart ���º��� �ٽ� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _destination = _startPosition;
            transform.position = _startPosition;
            _isDepart = false;
            State = GhostState.Depart;
            PrevState = GhostState.None;
        }
    }

    /// <summary>
    /// ������ �������� ����� �� ������ ������.
    /// �Ա��� ������ ������ �������� �������� �ʴ´�.
    /// </summary>
    private IEnumerator Depart()
    {
        if (_isDepart)
        {
            yield break;
        }

        // �� �����Ǿ��� ������ ������Ʈ Ǯ���� ���� ��ġ�� �־� ������ ��ġ�� ����
        _startPosition = transform.position;

        _isDepart = true;

        Move(Direction.Up);
        yield return new WaitForSeconds(1);
        Move(Direction.Right);
        yield return new WaitForSeconds(1);
        Move(Direction.Up);
        yield return new WaitForSeconds(1);
        Move(Direction.Up);
        yield return new WaitForSeconds(1);

        NowDirection = (Direction)Random.Range(3, 5);

        // ��� ������ ������ Walk ���·� ��ȯ
        PrevState = State;
        State = GhostState.Walk;
    }

    /// <summary>
    /// �˰����� ���� �����ϰ� ��ȸ�Ѵ�.
    /// </summary>
    private IEnumerator Walk()
    {
        // ���� ���� �� �̻��� �Ǹ� ���� ��ȯ
        if (_walkCount >= _randomNum)
        {
            _randomNum = Random.Range(5, 9);
            _walkCount = 0;

            NowDirection = (Direction)Random.Range(1, 5);
        }

        // ���� �ε����� ������ ��ȯ
        if (MapManager.Instance.CheckDirectionToGo(transform.position, NowDirection))
        {
            Move(NowDirection);
            _walkCount++;
        }
        else
        {
            NowDirection = (Direction)Random.Range(1, 5);
            _destination = transform.position;

            // ���� �ε����� ī��Ʈ �ʱ�ȭ
            _walkCount = 0;
        }

        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// ������ ������ ������ ��ǥ�� �����Ѵ�.
    /// </summary>
    /// <param name="direction">���ư� ����</param>
    private void Move(Direction direction)
    {
        ChangeDirection(direction);

        switch (direction)
        {
            case Direction.Up:
                _destination = transform.position + new Vector3(0f, _moveSpeed, 0f);
                break;
            case Direction.Down:
                _destination = transform.position + new Vector3(0f, -_moveSpeed, 0f);
                break;
            case Direction.Left:
                _destination = transform.position + new Vector3(-_moveSpeed, 0f, 0f);
                break;
            case Direction.Right:
                _destination = transform.position + new Vector3(_moveSpeed, 0f, 0f);
                break;
            default:
                break;
        }

        _directionToggle = true;
    }

    /// <summary>
    /// Ghost�� �� ������ Ż���ϴ� ���� �����ϱ� ����
    /// Ghost�� �� �� �ִ� �ִ� ������ �����ϸ� ������ ������ ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="direction">���ư� ����</param>
    private void ChangeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                if (transform.position.y == MapManager.MAP_SIZE_ROW - 2)
                {
                    NowDirection = Direction.Down;
                    _walkCount = 0;
                }
                break;
            case Direction.Down:
                if (transform.position.y == 1)
                {
                    NowDirection = Direction.Up;
                    _walkCount = 0;
                }
                break;
            case Direction.Left:
                if (transform.position.x == 1)
                {
                    NowDirection = Direction.Right;
                    _walkCount = 0;
                }
                break;
            case Direction.Right:
                if (transform.position.y == MapManager.MAP_SIZE_COLUMN - 2)
                {
                    NowDirection = Direction.Left;
                    _walkCount = 0;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ������ ��ǥ�� ���������� ����� �ε巴�� �̵��ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    private IEnumerator MoveSmoothly()
    {
        // ���� ��ǥ�� �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, _destination, _duration);

        // �����ؼ� �̵��ϴٺ��� ���� ��ǥ�� �������� ���� ���¿���
        // ���� ��ǥ�� �̵��ϴ� ���� ����µ� �̷��� �Ǹ� ���� ���� m�� �̵��� �� ����.
        if (Vector3.Distance(transform.position, _destination) <= ERROR_RANGE)
        {
            // �̸� �����ϱ� ����, ���� ��ǥ�� ��������� Player�� ��ġ�� ���� ��ǥ�� �̵� ��Ű�� ������ �ʿ��ϴ�.
            transform.position = new Vector3(Mathf.RoundToInt(_destination.x), Mathf.RoundToInt(_destination.y));
            _directionToggle = false;
        }
        yield return null;
    }
}
