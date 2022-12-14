using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Usually,
    Invincibility,
    Dead
}

public class PlayerMove : MonoBehaviour
{
    public PlayerState State;

    private const float ERROR_RANGE = 0.01f;

    private bool _directionToggle;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private Vector3 _destination;

    private bool _usedSkill = false;

    private PlayerInput _input;
    private Animator _animator;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
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
            // 입력에 따라 다른 방향을 지정
            if (_input.GoUp)
            {
                DirectionUp();
            }
            if (_input.GoDown)
            {
                DirectionDown();
            }
            if (_input.GoLeft)
            {
                DirectionLeft();
            }
            if (_input.GoRight)
            {
                DirectionRight();
            }
            if (!_usedSkill && _input.UseSkill)
            {
                StartCoroutine(OnInvincibility());
            }
        }
    }

    // 방향 지정 메서드
    private void DirectionUp()
    {
        // 플레이어가 범위 밖으로 벗어나려고 하면
        if (transform.position.y == MapManager.MAP_SIZE_ROW - 1)
        {
            // 반대쪽 범위 바깥에서 맵 안으로 들어오는 것처럼 보이게 한다.
            transform.position = new Vector3(transform.position.x, -1f, 0f);
            _destination = transform.position + new Vector3(0f, _moveSpeed, 0f);
            _directionToggle = true;
            return;
        }

        // 지정한 방향으로 나아갈 수 있는지 확인
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, Direction.Up))
        {
            return;
        }

        // 애니메이션 재생 설정
        _animator.SetBool(PlayerAnimID.Up, true);
        if (State == PlayerState.Invincibility)
        {
            _animator.SetBool(PlayerAnimID.InvincibilityUp, true);
        }

        // 방향에 따라 다른 좌표를 지정
        _destination = transform.position + new Vector3(0f, _moveSpeed, 0f);
        _directionToggle = true;
    }

    private void DirectionDown()
    {
        if (transform.position.y == 0)
        {
            transform.position = new Vector3(transform.position.x, MapManager.MAP_SIZE_ROW, 0f);
            _destination = transform.position + new Vector3(0f, -_moveSpeed, 0f);
            _directionToggle = true;
            return;
        }

        if (!MapManager.Instance.CheckDirectionToGo(transform.position, Direction.Down))
        {
            return;
        }

        _animator.SetBool(PlayerAnimID.Down, true);
        if (State == PlayerState.Invincibility)
        {
            _animator.SetBool(PlayerAnimID.InvincibilityDown, true);
        }

        _destination = transform.position + new Vector3(0f, -_moveSpeed, 0f);
        _directionToggle = true;
    }

    private void DirectionLeft()
    {
        if (transform.position.x == 0)
        {
            transform.position = new Vector3(MapManager.MAP_SIZE_COLUMN, transform.position.y, 0f);
            _destination = transform.position + new Vector3(-_moveSpeed, 0f, 0f);
            _directionToggle = true;
            return;
        }

        if (!MapManager.Instance.CheckDirectionToGo(transform.position, Direction.Left))
        {
            return;
        }

        _animator.SetBool(PlayerAnimID.Left, true);
        if (State == PlayerState.Invincibility)
        {
            _animator.SetBool(PlayerAnimID.InvincibilityLeft, true);
        }

        _destination = transform.position + new Vector3(-_moveSpeed, 0f, 0f);
        _directionToggle = true;
    }

    private void DirectionRight()
    {
        if (transform.position.x == MapManager.MAP_SIZE_COLUMN - 1)
        {
            transform.position = new Vector3(-1f, transform.position.y, 0f);
            _destination = transform.position + new Vector3(_moveSpeed, 0f, 0f);
            _directionToggle = true;
            return;
        }

        if (!MapManager.Instance.CheckDirectionToGo(transform.position, Direction.Right))
        {
            return;
        }

        _animator.SetBool(PlayerAnimID.Right, true);
        if (State == PlayerState.Invincibility)
        {
            _animator.SetBool(PlayerAnimID.InvincibilityRight, true);
        }

        _destination = transform.position + new Vector3(_moveSpeed, 0f, 0f);
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

            // 애니메이션도 리셋
            _animator.SetBool(PlayerAnimID.Up, false);
            _animator.SetBool(PlayerAnimID.Down, false);
            _animator.SetBool(PlayerAnimID.Left, false);
            _animator.SetBool(PlayerAnimID.Right, false);
            if (State == PlayerState.Invincibility)
            {
                _animator.SetBool(PlayerAnimID.InvincibilityUp, false);
                _animator.SetBool(PlayerAnimID.InvincibilityDown, false);
                _animator.SetBool(PlayerAnimID.InvincibilityLeft, false);
                _animator.SetBool(PlayerAnimID.InvincibilityRight, false);
            }
        }
        yield return null;
    }

    //토글을 끄는 public 함수
    //direction을 0,0,0으로 하는 public 함수
    public void OffDirectionToggle()
    {
        _directionToggle = false;
    }

    /// <summary>
    /// 2초간 무적 상태로 전환해주는 코루틴 함수
    /// </summary>
    private IEnumerator OnInvincibility()
    {
        _animator.SetBool(PlayerAnimID.UseSkill, true);

        State = PlayerState.Invincibility;
        yield return new WaitForSeconds(2);
        State = PlayerState.Usually;
        _usedSkill = true;
        _animator.SetBool(PlayerAnimID.UseSkill, false);
    }
}
