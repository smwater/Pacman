using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent PlayerDead;
    //�̺�Ʈ�� ������ �̱���
    public UnityEvent GameOver;
}
