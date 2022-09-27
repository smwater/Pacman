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
    //게임 시작시 start, ready, GameOver를 출력하는 Text
    public TextMeshProUGUI Text;
    public TextController InGameText;
    
    public UnityEvent PlayerDead;
    public UnityEvent<GameState> GameOver = new UnityEvent<GameState>();
    
    public GameObject RankingUIPrefab;
    private GameObject _rankingUI;
    private Vector3 RANKINGUI_POSITION = new Vector3(0, 0, 0);
    
    public bool IsPause = false;
    public bool _isClickYes = false;

    private GameState _gameState;

    private void OnEnable()
    {
        RankingUIPrefab.SetActive(false);
        GameOver.AddListener(InGameTextUI);
    }

    private void Start()
    {
        _gameState = GameState.Ready;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameOver.RemoveListener(InGameTextUI);
    }

    private void Update()
    {
        if(_isClickYes)
        {
            StartCoroutine(GameManager.Instance.PrintingText());
        }

        if (_gameState == GameState.Ready || _gameState == GameState.GameOver || _gameState == GameState.Clear)
        {
            IsPause = true;
        }
        if (_gameState == GameState.Playing)
        {
            IsPause = false;
        }
    }

    public IEnumerator PrintingText()
    {
        InGameTextUI(GameState.Ready);
        yield return new WaitForSecondsRealtime(3);
        InGameTextUI(GameState.Start);
        yield return new WaitForSecondsRealtime(1);
        InGameTextUI(GameState.Playing);
        _isClickYes = false;
        Debug.Log("코루틴 종료");
    }

    private void InGameTextUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Ready:
                InGameText.On();
                Text.text = "Ready";
                break;
            case GameState.Start:
                Text.text = "Start";
                Text.color = Color.red;
                _gameState = GameState.Playing;
                break;
            case GameState.Playing:
                InGameText.Off();
                break;
            case GameState.Clear:
                Text.text = "Clear";
                Text.color = Color.yellow;
                _gameState = GameState.Clear;
                InGameText.On();
                Invoke("ShowRankingUI", 3);
                break;
            case GameState.GameOver:
                Text.text = "Game Over";
                Text.color = Color.red;
                _gameState = GameState.GameOver;
                InGameText.On();
                Invoke("ShowRankingUI", 3);
                break;
        }
    }

    private void ShowRankingUI()
    {
        _rankingUI = Instantiate(RankingUIPrefab, Vector3.zero, Quaternion.identity);
        _rankingUI.gameObject.SetActive(true);
    }
}