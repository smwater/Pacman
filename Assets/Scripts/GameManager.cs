using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent PlayerDead;
    //이벤트가 있으나 미구현
    public UnityEvent GameOver;
}
