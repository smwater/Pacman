using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class StageScelectManager : MonoBehaviour
{
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

    //��ư�� �̸��� �����ϴ� �Լ�
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

    /// <summary>
    /// Yes��ư Ŭ���� ���ӸŴ����� InGameUI�� Ȱ��ȭ ��Ŵ
    /// </summary>
    public void ClickYesButton()
    {
        MapManager.Instance.LoadStage1();
        GameManager.Instance._isClickYes = true;
        gameObject.SetActive(false);
    }

    public void ClickNoButton()
    {
        _selectUI.SetActive(false);
    }

}
