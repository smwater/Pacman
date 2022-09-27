using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ ������ �����ϴ� ������ Ÿ��
public enum CoinScore
{
    Small = 10,
    Big = 50
}

public class Coin : MonoBehaviour
{
    public CoinScore CoinSize;

    // �÷��̾�� �浹���� �� ȹ�� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            MapManager.Instance.CountCoin(CoinSize);
        }
    }
}
