using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageScelectManager : MonoBehaviour
{   
    private GameObject SelectWindow;
    private bool _selectWindowActive = false;

    private bool _isEasy;

    private void Awake()
    {
        SelectWindow = GameObject.Find("SelectWindows");
        SelectWindow.transform.GetChild(0).gameObject.SetActive(false);
        SelectWindow.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene("Title");
    }

    //버튼의 이름을 리턴하는 함수
    private string GetButtonName()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        _isEasy = buttonName;
        return buttonName;
    }

    public void ClickSelectButton()
    {
        if(!_selectWindowActive)
        {
            if(GetButtonName() == "Easy")
            {
                SelectWindow.transform.GetChild(0).gameObject.SetActive(true);
                    _selectWindowActive = true;
            }
            else
            {
                SelectWindow.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Yes버튼 클릭시 게임매니저의 InGameUI를 활성화 시킴
    /// </summary>
    public void ClickYesButton()
    {
        if(_isEasy)
        {
            MapManager.Instance.LoadStage1();
        }
        else
        {
            //MapManager.Instance.LoadStage2();
        }

        GameManager.Instance._isClickYes = true;
        gameObject.SetActive(false);
    }

    public void ClickNoButton()
    {
        SelectWindow.transform.GetChild(0).gameObject.SetActive(false);
        SelectWindow.transform.GetChild(1).gameObject.SetActive(false);

        _selectWindowActive = false;
    }

}
