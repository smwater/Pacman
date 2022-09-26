using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float ERROR_RANGE = 0.01f;

    private bool _directionToggle;
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private Vector3 _destination;

    private PlayerInput _input;
    private bool _isPlayerDead = false;


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
        if (_input.UseSkill)
        {
            Debug.Log("스킬은 아직 미구현");
        }

    }

    // 방향 지정 메서드
    private void DirectionUp()
    {
        // 지정한 방향으로 나아갈 수 있는지 확인
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, MapManager.Direction.Up))
        {
            return;
        }

        // 애니메이션 세팅도 여기서 할 예정
        // 방향에 따라 다른 좌표를 지정
        _destination = transform.position + new Vector3(0f, _speed, 0f);
        _directionToggle = true;
    }

    private void DirectionDown()
    {
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, MapManager.Direction.Down))
        {
            return;
        }

        _destination = transform.position + new Vector3(0f, -_speed, 0f);
        _directionToggle = true;
    }

    private void DirectionLeft()
    {
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, MapManager.Direction.Left))
        {
            return;
        }

        _destination = transform.position + new Vector3(-_speed, 0f, 0f);
        _directionToggle = true;
    }

    private void DirectionRight()
    {
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, MapManager.Direction.Right))
        {
            return;
        }

        _destination = transform.position + new Vector3(_speed, 0f, 0f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ghost"))
        {
            _isPlayerDead = true;
        }
    }
}
