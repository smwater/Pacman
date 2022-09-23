using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    private bool _isWall = false;

    // ������ ���̱� ���� OnTriggerStay�� ������� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            _isWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            _isWall = false;
        }
    }

    // ������ ���� �浹�ߴ��� �˷��ִ� �޼���
    public bool AnnounceIsWall()
    {
        return _isWall;
    }
}
