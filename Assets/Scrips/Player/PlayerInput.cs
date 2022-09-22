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
        GoUp = GoDown = GoLeft = GoRight = UseSkill = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        { 
            GoUp = true;
            Debug.Log("UP");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoDown = true;
            Debug.Log("DOWN");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GoLeft = true;
            Debug.Log("LEFT");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GoRight = true;
            Debug.Log("RIGHT");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill = true;
            Debug.Log("SKILL");
        }
    }
}
