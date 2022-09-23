using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ���� �÷��̾ ��� ������ ���ߴ��� �����ϴ� ������ Ÿ�� ����
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    private Direction _prensent = Direction.None;
    private const float ERROR_RANGE = 0.01f;

    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private bool _directionToggle;
    private Vector3 _destination;
    private PlayerInput _input;

    private GameObject[] _sensors;
    private int _sensorCount;
    private PlayerNavigation[] _playerNavigations;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();

        // �÷��̾� ���� ������Ʈ�� �����ϴ� ������ ������ �����ϴ� ������Ʈ�� ����
        _sensorCount = transform.childCount;
        _sensors = new GameObject[_sensorCount];
        _playerNavigations = new PlayerNavigation[_sensorCount];
        for (int i = 0; i < _sensorCount; i++)
        {
            _sensors[i] = transform.GetChild(i).gameObject;
            _playerNavigations[i] = _sensors[i].GetComponent<PlayerNavigation>();
        }
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

        // ������ �����Ǿ��� ���� ���� ����
        if (_prensent != Direction.None)
        {
            UseNavigation();
        }
    }

    // ���� ���� �޼���
    private void DirectionUp()
    {
        _prensent = Direction.Up;
        // ���⿡ ���� �ٸ� ��ǥ�� ����
        _destination = transform.position + new Vector3(0f, _speed, 0f);
    }

    private void DirectionDown()
    {
        _prensent = Direction.Down;
        _destination = transform.position + new Vector3(0f, -_speed, 0f);
    }

    private void DirectionLeft()
    {
        _prensent = Direction.Left;
        _destination = transform.position + new Vector3(-_speed, 0f, 0f);
    }

    private void DirectionRight()
    {
        _prensent = Direction.Right;
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

    /// <summary>
    /// ������ ���⿡ ���� �����ϴ��� �Ǵ��ϰ�,
    /// ���� �����Ѵٸ� ������ ��ǥ�� �����Ѵ�.
    /// </summary>
    private void UseNavigation()
    {
        if (!_playerNavigations[(int)_prensent].AnnounceIsWall())
        {
            return;
        }

        _destination = transform.position;
        _directionToggle = false;
    }
}