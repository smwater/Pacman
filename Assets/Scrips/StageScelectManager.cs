using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class StageScelectManager : MonoBehaviour
{
    
    public TextMeshProUGUI SelectiveQuestions;
    
    GameObject SelectUI;

    private void Awake()
    {
        SelectUI = GameObject.Find("SelectUI");

    }

    private void Start()
    {
        SelectUI.SetActive(false);
    }

    private string GetButtonName()
    {
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        return ButtonName;
    }


    public void ClickBackButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void ClickSelectButton()
    {
        SelectUI.SetActive(true);
        SelectiveQuestions.text = $"Do you want to select that stage?({GetButtonName()})";
    }

    public void ClickYesButton()
    {
        gameObject.SetActive(false);
    }

    public void ClickNoButton()
    {
        SelectUI.SetActive(false);
    }

}
