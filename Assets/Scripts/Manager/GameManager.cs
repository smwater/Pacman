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

    //InGameUI오브젝트
    public GameObject InGameUI;

    //InGameUI 스크립트 참조를 위한 오브젝트
    //InGameUI의 자식으로 받는 조치 필요
    public InGameUI InGameUIObject;
    
    //게임이 멈췄는지
    public bool IsPause = false;
    //스테이지 선택 Yes버튼이 눌렸는지 
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
        if (Input.GetKeyDown(KeyCode.Escape) && _gameState == GameState.Playing)
        {
            IsPause = true;
            InGameUIObject.OnPauseUI();
        }

        if(_isClickYes)
        {
            InGameUI.SetActive(true);
            StartCoroutine(GameManager.Instance.PrintingText());
        }
    }

    /// <summary>
    ///  게임 상태에 따라 GameState UI를 출력하고 해당 상태마다 지연하는 함수
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrintingText()
    {
        InGameTextUI(GameState.Ready);
        yield return new WaitForSecondsRealtime(3);
        InGameTextUI(GameState.Start);
        yield return new WaitForSecondsRealtime(1);
        InGameTextUI(GameState.Playing);
        _isClickYes = false;
    }

    /// <summary>
    /// GameState에 따라 InGameUI를 변경하는 함수
    /// 게임이 끝나는 경우(클리어, 게임오버) 3초 뒤 랭킹창 팝업
    /// </summary>
    /// <param name="gameState"></param>
    private void InGameTextUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Ready:
                InGameText.On();
                Text.text = "Ready";
                InGameUI.SetActive(true);
                IsPause = true;
                break;
            case GameState.Start:
                Text.text = "Start";
                Text.color = Color.red;
                _gameState = GameState.Playing;
                break;
            case GameState.Playing:
                InGameText.Off();
                IsPause = false;
                break;
            case GameState.Clear:
                Text.text = "Clear";
                Text.color = Color.yellow;
                _gameState = GameState.Clear;
                InGameText.On();
                IsPause = true;
                Invoke("ShowRankingUI", 3);
                break;
            case GameState.GameOver:
                Text.text = "Game Over";
                Text.color = Color.red;
                _gameState = GameState.GameOver;
                InGameText.On();
                IsPause = true;
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