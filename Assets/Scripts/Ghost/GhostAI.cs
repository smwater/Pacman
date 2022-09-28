using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public Direction NowDirection;

    private const float ERROR_RANGE = 0.01f;

    private bool _directionToggle;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _duration = 0.1f;

    private Vector3 _destination;
    private Vector3 _startPosition;

    private int _walkCount = 0;
    private int _randomNum = 1;

    private void Awake()
    {
        NowDirection = (Direction)Random.Range(1, 5);
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

            StartCoroutine(Walk());
        }
    }

    // ������ �÷��̾ ����� ���, ������ ����� ���� ��ǥ�� �����ϰ�
    // Walk ���º��� �ٽ� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾ ���� ���¶�� ����
            PlayerState playerState = collision.GetComponent<PlayerMove>().State;
            if (playerState == PlayerState.Invincibility)
            {
                return;
            }

            _destination = _startPosition;
            transform.position = _startPosition;
            NowDirection = (Direction)Random.Range(1, 5);
        }
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
        ChangeDirection();

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
    /// Ghost�� ������������ ��ġ�� �����ϸ� ������ ������ ��ȯ�Ѵ�.
    /// </summary>
    private void ChangeDirection()
    {
        if (transform.position.y == MapManager.MAP_SIZE_ROW - 2 && transform.position.x >= 14 && transform.position.x <= 15)
        {
            // �������� �����ϱ� ���� ���������� ���� ���� �߿��� �����Ѵ�.
            NowDirection = (Direction)Random.Range(2, 5);
            _walkCount = 0;
        }

        if (transform.position.y == 1 && transform.position.x >= 14 && transform.position.x <= 15)
        {
            do
            {
                NowDirection = (Direction)Random.Range(1, 5);
            } while (NowDirection == Direction.Down);

            _walkCount = 0;
        }

        if (transform.position.x == 1 && transform.position.y >= 14 && transform.position.y <= 15)
        {
            do
            {
                NowDirection = (Direction)Random.Range(1, 5);
            } while (NowDirection == Direction.Left);

            _walkCount = 0;
        }

        if (transform.position.y == MapManager.MAP_SIZE_COLUMN - 2 && transform.position.y >= 14 && transform.position.y <= 15)
        {
            NowDirection = (Direction)Random.Range(1, 4);
            _walkCount = 0;
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

    /// <summary>
    /// ������ ���� ��ġ�� �������ִ� �Լ�
    /// </summary>
    /// <param name="x">������ ���� ��ǥ�� x ��</param>
    /// <param name="y">������ ���� ��ǥ�� y ��</param>
    public void SetStartPosition(float x, float y)
    {
        _startPosition = new Vector2(x, y);
    }
}
