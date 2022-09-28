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

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
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
            // �Է¿� ���� �ٸ� ������ ����
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

    // ���� ���� �޼���
    private void DirectionUp()
    {
        // �÷��̾ ���� ������ ������� �ϸ�
        if (transform.position.y == MapManager.MAP_SIZE_ROW - 1)
        {
            // �ݴ��� ���� �ٱ����� �� ������ ������ ��ó�� ���̰� �Ѵ�.
            transform.position = new Vector3(transform.position.x, -1f, 0f);
            _destination = transform.position + new Vector3(0f, _moveSpeed, 0f);
            _directionToggle = true;
            return;
        }

        // ������ �������� ���ư� �� �ִ��� Ȯ��
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, Direction.Up))
        {
            return;
        }

        // �ִϸ��̼� ���õ� ���⼭ �� ����
        // ���⿡ ���� �ٸ� ��ǥ�� ����
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

        _destination = transform.position + new Vector3(_moveSpeed, 0f, 0f);
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

    //����� ���� public �Լ�
    //direction�� 0,0,0���� �ϴ� public �Լ�
    public void OffDirectionToggle()
    {
        _directionToggle = false;
    }

    /// <summary>
    /// 2�ʰ� ���� ���·� ��ȯ���ִ� �ڷ�ƾ �Լ�
    /// </summary>
    private IEnumerator OnInvincibility()
    {
        State = PlayerState.Invincibility;
        yield return new WaitForSeconds(2);
        State = PlayerState.Usually;
        _usedSkill = true;
    }
}
