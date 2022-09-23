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
            Debug.Log("��ų�� ���� �̱���");
        }
    }

    // ���� ���� �޼���
    private void DirectionUp()
    {
        // ���⿡ ���� �ٸ� ��ǥ�� ����
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