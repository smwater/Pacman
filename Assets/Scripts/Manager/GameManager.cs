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
    //���� ���۽� start, ready, GameOver�� ����ϴ� Text
    public TextMeshProUGUI Text;
    public TextController InGameText;
    
    public UnityEvent PlayerDead;
    public UnityEvent<GameState> GameOver = new UnityEvent<GameState>();
    
    public GameObject RankingUIPrefab;
    private GameObject _rankingUI;

    //InGameUI������Ʈ
    public GameObject InGameUI;

    //InGameUI ��ũ��Ʈ ������ ���� ������Ʈ
    //InGameUI�� �ڽ����� �޴� ��ġ �ʿ�
    public InGameUI InGameUIObject;
    
    //������ �������
    public bool IsPause = false;
    //�������� ���� Yes��ư�� ���ȴ��� 
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
    ///  ���� ���¿� ���� GameState UI�� ����ϰ� �ش� ���¸��� �����ϴ� �Լ�
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
    /// GameState�� ���� InGameUI�� �����ϴ� �Լ�
    /// ������ ������ ���(Ŭ����, ���ӿ���) 3�� �� ��ŷâ �˾�
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