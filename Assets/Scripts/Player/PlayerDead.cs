using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    Alive,
    Dead
}

public class PlayerDead : MonoBehaviour
{
    public int _playerLife = 4;
    private PlayerState _playerState;

    private Vector3 _initPosition;

    private void Awake()
    {
        _initPosition = GetComponent<Transform>().position;
    }

    private void Start()
    {
        _playerState = PlayerState.Alive;
    }

    private void Update()
    {
        if(_playerState == PlayerState.Dead)
        { 
            DecreaseLife();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            Debug.Log("부딪힘");
        if (collision.CompareTag("Ghost"))
        {
            _playerState = PlayerState.Dead;
        }
        
    }
    
    /// <summary>
    /// 플레이어의 라이프를 1 감소시키고 위치 초기화, 라이프가 없을 경우 게임오버
    /// </summary>
    private void DecreaseLife()
    {
        if (_playerLife <= 0)
        {
            //게임 오버 처리
            Debug.Log("게임 오버");
        }
        else
        {
            --_playerLife;
            transform.position = new Vector3(0,0,0);
            _playerState = PlayerState.Alive;
            Debug.Log($"{_playerLife}");
            Debug.Log($"{_playerState}");
        }
        
    }
}
