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
    /// �÷��̾ ���ɿ��� ����� �� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Dead()
    {
        _move.OffDirectionToggle();
        GameManager.Instance.PlayerDead.Invoke();
    }

    /// <summary>
    /// �÷��̾��� �������� 1 ���ҽ�Ű�� ��ġ �ʱ�ȭ, �������� ���� ��� ���ӿ���
    /// </summary>
    private void DecreaseLife()
    {
        if (_life <= 0)
        {
            Debug.Log("���� ����");
            GameManager.Instance.GameOver.Invoke();
        }
        else
        {
            --_life;
            Dead();
        }
    }
}
