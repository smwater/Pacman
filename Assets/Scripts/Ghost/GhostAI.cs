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
            // 방향이 지정되었을 때만 이동
            if (_directionToggle)
            {
                StartCoroutine(MoveSmoothly());
                return;
            }

            StartCoroutine(Walk());
        }
    }

    // 유령이 플레이어를 잡았을 경우, 가려던 방향과 현재 좌표를 리셋하고
    // Walk 상태부터 다시 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _destination = _startPosition;
            transform.position = _startPosition;
            NowDirection = (Direction)Random.Range(1, 5);
        }
    }

    /// <summary>
    /// 알고리즘을 따라 랜덤하게 배회한다.
    /// </summary>
    private IEnumerator Walk()
    {
        // 일정 걸음 수 이상이 되면 방향 전환
        if (_walkCount >= _randomNum)
        {
            _randomNum = Random.Range(5, 9);
            _walkCount = 0;

            NowDirection = (Direction)Random.Range(1, 5);
        }

        // 벽에 부딪히면 방향을 전환
        if (MapManager.Instance.CheckDirectionToGo(transform.position, NowDirection))
        {
            Move(NowDirection);
            _walkCount++;
        }
        else
        {
            NowDirection = (Direction)Random.Range(1, 5);
            _destination = transform.position;

            // 벽에 부딪히면 카운트 초기화
            _walkCount = 0;
        }

        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// 지정한 방향을 목적지 좌표로 저장한다.
    /// </summary>
    /// <param name="direction">나아갈 방향</param>
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
    /// Ghost가 맵 밖으로 탈출하는 것을 방지하기 위해
    /// Ghost가 갈 수 있는 최대 범위에 도달하면 방향을 강제로 전환한다.
    /// </summary>
    /// <param name="direction">나아갈 방향</param>
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
    /// 지정된 좌표로 선형보간을 사용해 부드럽게 이동하는 코루틴 함수
    /// </summary>
    private IEnumerator MoveSmoothly()
    {
        // 지정 좌표로 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, _destination, _duration);

        // 연속해서 이동하다보면 지정 좌표에 도착하지 않은 상태에서
        // 다음 좌표로 이동하는 일이 생기는데 이렇게 되면 정수 단위 m로 이동할 수 없다.
        if (Vector3.Distance(transform.position, _destination) <= ERROR_RANGE)
        {
            // 이를 방지하기 위해, 지정 좌표에 가까워지면 Player의 위치를 지정 좌표로 이동 시키는 과정이 필요하다.
            transform.position = new Vector3(Mathf.RoundToInt(_destination.x), Mathf.RoundToInt(_destination.y));
            _directionToggle = false;
        }
        yield return null;
    }

    /// <summary>
    /// 유령의 시작 위치를 설정해주는 함수
    /// </summary>
    /// <param name="x">유령의 시작 좌표의 x 값</param>
    /// <param name="y">유령의 시작 좌표의 y 값</param>
    public void SetStartPosition(float x, float y)
    {
        _startPosition = new Vector2(x, y);
    }
}
