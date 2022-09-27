using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코인의 종류와 점수를 저장하는 열거형 타입
public enum CoinScore
{
    Small = 10,
    Big = 50
}

public class Coin : MonoBehaviour
{
    public CoinScore CoinSize;

    // 플레이어와 충돌했을 때 획득 판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            MapManager.Instance.CountCoin(CoinSize);
        }
    }
}
