using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool GoUp { get; private set; }
    public bool GoDown { get; private set; }
    public bool GoLeft { get; private set; }
    public bool GoRight { get; private set; }
    public bool UseSkill { get; private set; }

    private void Update()
    {
        // �Է��� ���� �����ӱ��� ������ ���� �ʱ� ���� ����
        GoUp = GoDown = GoLeft = GoRight = UseSkill = false;

        // ������ Ű �Է�
        if (Input.GetKey(KeyCode.UpArrow))
        { 
            GoUp = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GoDown = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GoLeft = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GoRight = true;
        }
    }
}
