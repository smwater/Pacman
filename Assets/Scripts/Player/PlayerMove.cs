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
        if (_input.UseSkill)
        {
            Debug.Log("��ų�� ���� �̱���");
        }

    }

    // ���� ���� �޼���
    private void DirectionUp()
    {
        // ������ �������� ���ư� �� �ִ��� Ȯ��
        if (!MapManager.Instance.CheckDirectionToGo(transform.position, MapManager.Direction.Up))
        {
            return;
        }

        // �ִϸ��̼� ���õ� ���⼭ �� ����
        // ���⿡ ���� �ٸ� ��ǥ�� ����
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ghost"))
        {
            _isPlayerDead = true;
        }
    }
}
