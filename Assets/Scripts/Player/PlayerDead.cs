using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField]
    private int _life = 4;

    private PlayerMove _move;

    private void Awake()
    {
        _move = GetComponent<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ghost"))
        {
            DecreaseLife();
        }
    }

    /// <summary>
    /// 플레이어가 유령에게 닿았을 때 호출되는 함수
    /// </summary>
    private void Dead()
    {
        _move.OffDirectionToggle();
        GameManager.Instance.PlayerDead.Invoke();
    }

    /// <summary>
    /// 플레이어의 라이프를 1 감소시키고 위치 초기화, 라이프가 없을 경우 게임오버
    /// </summary>
    private void DecreaseLife()
    {
        if (_life <= 0)
        {
            Debug.Log("게임 오버");
            GameManager.Instance.GameOver.Invoke();
        }
        else
        {
            --_life;
            Dead();
        }
    }
}
