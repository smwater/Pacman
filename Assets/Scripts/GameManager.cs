using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent PlayerDead;
    public UnityEvent GameOver;

    private void RoadRankingScene()
    {
        SceneManager.LoadScene("Ranking");
    }
}
