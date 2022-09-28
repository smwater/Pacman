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
        // �Ϲ� ������ ���� ������ ����
        if (_move.State == PlayerState.Usually && collision.CompareTag("Ghost"))
        {
            DecreaseLife();
        }
    }

    /// <summary>
    /// �÷��̾ ���ɿ��� ����� �� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Dead()
    {
        --_life;
        _move.State = PlayerState.Dead;
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
            GameManager.Instance.GameOver.Invoke(GameState.GameOver);
        }
        else
        {
            Dead();
        }
    }
}
