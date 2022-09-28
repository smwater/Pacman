using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private GameObject _pauseUI;
    private GameObject _earlyWarningUI;

    private void Awake()
    {
        _pauseUI = GameObject.Find("PauseImage");
        _earlyWarningUI = GameObject.Find("EarlyWarningUI");
    }

    private void Start()
    {
        _pauseUI.SetActive(false);
        _earlyWarningUI.SetActive(false);
    }

    public void OnPauseUI()
    {
        _pauseUI.SetActive(true);
    }

    public void ClickGetOutButton()
    {
        _earlyWarningUI.SetActive(true);
    }

    public void ClickClosePause()
    {
        _pauseUI.SetActive(false);
        _earlyWarningUI.SetActive(false);
        GameManager.Instance.IsPause = false;
        GameManager.Instance._audioSource.Play();
    }

    public void ClickOkButton()
    {
        SceneManager.LoadScene("Title");
    }
}
