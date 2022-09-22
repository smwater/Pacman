using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private readonly float _speed = 1f;
    [SerializeField]
    private float _duration = 0.1f;
    private bool _isMove;
    private Vector3 _destination;

    private PlayerInput _input;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_isMove)
        {
            StartCoroutine(MoveSmoothly());
            return;
        }

        if (_input.GoUp)
        {
            DirectionUp();
            _isMove = true;
        }
        if (_input.GoDown)
        {
            DirectionDown();
            _isMove = true;
        }
        if (_input.GoLeft)
        {
            DirectionLeft();
            _isMove = true;
        }
        if (_input.GoRight)
        {
            DirectionRight();
            _isMove = true;
        }
        if (_input.UseSkill)
        {
            Debug.Log("스킬은 아직 미구현");
        }
    }

    private void DirectionUp()
    {
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

    private IEnumerator MoveSmoothly()
    {
        transform.position = Vector3.Lerp(transform.position, _destination, _duration);
        yield return null;
        if (Vector3.Distance(transform.position, _destination) <= 0.01f)
        {
            transform.position = new Vector3(Mathf.RoundToInt(_destination.x), Mathf.RoundToInt(_destination.y));
            _isMove = false;
        }
    }
}