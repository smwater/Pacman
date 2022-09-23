using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class StageScelectManager : MonoBehaviour
{
    //원하는 Object를 드래그엔 드랍으로 어떻게 넣을지 몰라서 public 선언
    public TextMeshProUGUI SelectiveQuestions;
    
    private GameObject _selectUI;

    private void Awake()
    {
        _selectUI = GameObject.Find("SelectUI");
    }

    private void Start()
    {
        _selectUI.SetActive(false);
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene("Title");
    }

    //버튼의 이름을 리턴하는 함수
    private string GetButtonName()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        return buttonName;
    }

    public void ClickSelectButton()
    {
        _selectUI.SetActive(true);
        SelectiveQuestions.text = $"Do you want to select that stage?({GetButtonName()})";
    }

    public void ClickYesButton()
    {
        gameObject.SetActive(false);
    }

    public void ClickNoButton()
    {
        _selectUI.SetActive(false);
    }

}
