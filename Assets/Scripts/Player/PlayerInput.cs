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
        // 입력이 다음 프레임까지 영향을 주지 않기 위해 리셋
        GoUp = GoDown = GoLeft = GoRight = UseSkill = false;

        // 각각의 키 입력
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
