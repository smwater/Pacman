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
    GameOver
}

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent PlayerDead;

    public UnityEvent<GameState> GameOver = new UnityEvent<GameState>();

    //게임 시작시 start, ready, GameOver를 출력하는 Text
    public TextMeshProUGUI InGameText;

    public GameObject RankingUIPrefab;

    private GameState _gameState = GameState.None;

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
        _gameState = GameState.Ready;
        InGameText.gameObject.SetActive(true);
        StartCoroutine(PrintingText(_gameState));
    }

    private void OnDisable()
    {
        GameManager.Instance.GameOver.RemoveListener(InGameTextUI);
    }

    private IEnumerator PrintingText(GameState gameState)
    {
        InGameTextUI(GameState.Ready);
        yield return new WaitForSeconds(3);
        InGameTextUI(GameState.Start);
        yield return new WaitForSeconds(1);
        InGameTextUI(GameState.Playing);
    }

    private void InGameTextUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Ready:
                InGameText.text = "Ready";
                _gameState = GameState.Start;
                break;
            case GameState.Start:
                InGameText.text = "Start";
                break;
            case GameState.Playing:
                InGameText.gameObject.SetActive(false);
                break;
            case GameState.GameOver:
                InGameText.text = "Game Over";
                InGameText.gameObject.SetActive(true);
                RankingUIPrefab.gameObject.SetActive(true);
                Debug.Log($"{RankingUIPrefab.gameObject.activeSelf}");
                break;
        }
    }
}
