using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageScelectManager : MonoBehaviour
{   
    private GameObject _selectUI;
    private Sprite[] sprites;

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
        if(GetButtonName() == "Easy")
        {
            //_selectUI.GetComponent<Image>().sprite = sprites[0];
        }

        _selectUI.SetActive(true);
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
