using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public enum GameState
{
    None,
    Ready,
    Start,
    Playing,
    Clear,
    GameOver
}

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent PlayerDead;

    public UnityEvent<GameState> GameOver = new UnityEvent<GameState>();

    //게임 시작시 start, ready, GameOver를 출력하는 Text
    public TextMeshProUGUI InGameText;

    public GameObject RankingUIPrefab;

    public bool IsPause = false;

    private GameState _gameState;

    private void Awake()
    {
        RankingUIPrefab.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.GameOver.AddListener(InGameTextUI);
    }

    private void Start()
    {
        InGameText.gameObject.SetActive(true);
        StartCoroutine(PrintingText());
        _gameState = GameState.Ready;
    }

    private void Update()
    {
        if(_gameState == GameState.Ready)
        {
            IsPause = true;
            Time.timeScale = 0;
        }
        if(_gameState == GameState.Playing)
        {
            IsPause = false;
            Time.timeScale = 1;
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.GameOver.RemoveListener(InGameTextUI);
    }

    private IEnumerator PrintingText()
    {
        InGameTextUI(GameState.Ready);
        yield return new WaitForSecondsRealtime(3);
        InGameTextUI(GameState.Start);
        yield return new WaitForSecondsRealtime(1);
        InGameTextUI(GameState.Playing);
    }

    private void ShowRankingUI()
    {
        RankingUIPrefab.gameObject.SetActive(true);
    }

    private void InGameTextUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Ready:
                InGameText.text = "Ready";
                break;
            case GameState.Start:
                InGameText.text = "Start";
                _gameState = GameState.Playing;
                break;
            case GameState.Playing:
                InGameText.gameObject.SetActive(false);
                break;
            case GameState.Clear:
                InGameText.text = "Clear";
                InGameText.gameObject.SetActive(true);
                Invoke("ShowRankingUI", 3);
                break;
            case GameState.GameOver:
                InGameText.text = "Game Over";
                InGameText.gameObject.SetActive(true);
                Invoke("ShowRankingUI", 3);
                break;
        }
    }
}
