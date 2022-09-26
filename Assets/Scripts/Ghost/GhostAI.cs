using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    None,
    Depart,  // ���
    Walk,   // ���ƴٴϱ�
    Chase,  // ����
    Shrink, // �̸���
    Dead    // ���
}

public class GhostAI : MonoBehaviour
{
    public GhostState State;
    public GhostState PrevState = GhostState.None;
    public Direction NowDirection;
    public Direction PrevDirection = Direction.None;

    private const float ERROR_RANGE = 0.01f;

    private bool _directionToggle;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private Vector3 _destination;

    private Vector3 _startPosition;
    private bool _isDepart = false;

    private bool _foundPlayer;

    private void Awake()
    {
        State = GhostState.Depart;
    }

    private void Update()
    {
        if (_directionToggle)
        {
            StartCoroutine(MoveSmoothly());
            return;
        }

        if (!_isDepart && State == GhostState.Depart)
        {
            StartCoroutine(Depart());
            return;
        }

        if (State != PrevState)
        {
            switch (State)
            {
                case GhostState.Walk: StartCoroutine(Walk()); break;
                case GhostState.Chase: Chase(); break;
                case GhostState.Shrink: Shrink(); break;
                case GhostState.Dead: Dead(); break;
                default: break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _destination = _startPosition;
            transform.position = _startPosition;
            State = GhostState.Depart;
            StartCoroutine(Depart());
        }
    }

    /// <summary>
    /// ������ �������� ����� �� ������ ������.
    /// �Ա��� ������ ������ �������� �������� �ʴ´�.
    /// </summary>
    private IEnumerator Depart()
    {
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

        NowDirection = Direction.Left;
        PrevState = State;
        State = GhostState.Walk;
    }

    /// <summary>
    /// �÷��̾ ã�� ������ �����ϰ� ��ȸ�Ѵ�.
    /// </summary>
    private IEnumerator Walk()
    {
        if (_foundPlayer)
        {
            PrevState = State;
            State = GhostState.Chase;
        }

        if (MapManager.Instance.CheckDirectionToGo(transform.position, NowDirection))
        {
            Move(NowDirection);
        }
        else
        {
            NowDirection = (Direction)Random.Range(1, 5);
            _destination = transform.position;
        }

        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// �÷��̾ �������� ����� ������ �����Ѵ�.
    /// </summary>
    private void Chase()
    {
        
    }

    /// <summary>
    /// �̸��� ����. ��Ƹ����� �ʱ� ���� �÷��̾�κ��� �־�����.
    /// </summary>
    private void Shrink()
    {

    }

    /// <summary>
    /// ���� ����. ���� ���� ä�� ������ �������� ���ư���.
    /// </summary>
    private void Dead()
    {

    }

    private void Move(Direction direction)
    {
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
