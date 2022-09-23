using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float ERROR_RANGE = 0.01f;

    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private bool _directionToggle;
    private Vector3 _destination;
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
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
            _directionToggle = true;
        }
        if (_input.GoDown)
        {
            DirectionDown();
            _directionToggle = true;
        }
        if (_input.GoLeft)
        {
            DirectionLeft();
            _directionToggle = true;
        }
        if (_input.GoRight)
        {
            DirectionRight();
            _directionToggle = true;
        }
        if (_input.UseSkill)
        {
            Debug.Log("스킬은 아직 미구현");
        }
    }

    // 방향 지정 메서드
    private void DirectionUp()
    {
        // 방향에 따라 다른 좌표를 지정
        _destination = transform.position + new Vector3(0f, _speed, 0f);
    }

    private void DirectionDown()
    {
        _destination = transform.position + new Vector3(0f, -_speed, 0f);
    }

    private void DirectionLeft()
    {
        _destination = transform.position + new Vector3(-_speed, 0f, 0f);
    }

    private void DirectionRight()
    {
        _destination = transform.position + new Vector3(_speed, 0f, 0f);
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