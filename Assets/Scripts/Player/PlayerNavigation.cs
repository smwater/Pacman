using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    private bool _isWall = false;

    // 연산을 줄이기 위해 OnTriggerStay를 사용하지 않음
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

    // 센서가 벽과 충돌했는지 알려주는 메서드
    public bool AnnounceIsWall()
    {
        return _isWall;
    }
}
