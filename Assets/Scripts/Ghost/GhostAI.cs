using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    None,
    Depart,  // 출발
    Walk,   // 돌아다니기
    Chase,  // 추적
    Shrink, // 겁먹음
    Dead    // 사망
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
    /// 리스폰 지점에서 출발해 문 밖으로 나간다.
    /// 입구로 완전히 나가기 전까지는 추적하지 않는다.
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
    /// 플레이어를 찾을 때까지 랜덤하게 배회한다.
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
    /// 플레이어가 범위에서 벗어나기 전까지 추적한다.
    /// </summary>
    private void Chase()
    {
        
    }

    /// <summary>
    /// 겁먹음 상태. 잡아먹히지 않기 위해 플레이어로부터 멀어진다.
    /// </summary>
    private void Shrink()
    {

    }

    /// <summary>
    /// 죽은 상태. 눈만 남은 채로 리스폰 지점으로 돌아간다.
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
}
