using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    private float titleMoveSpeed;
    private GameObject titleImage;
    private float titleMovePosition_Y;
    private GameObject button;
    private GameObject helpButton;
    
    //팝업 창
    private GameObject descriptionUI;
    private GameObject creditUI;


    private void Awake()
    {
        titleImage = GameObject.Find("TitileImage");
        button = GameObject.Find("Button");
        helpButton = GameObject.Find("HelpButton");
        
        descriptionUI = GameObject.Find("DescriptionUI");
        creditUI = GameObject.Find("CreditUI");
    }

    void Start()
    {
        button.SetActive(false);
        descriptionUI.SetActive(false);
        creditUI.SetActive(false);
    }

    void Update()
    {
        TitleMove();
    }

    private void TitleMove()
    {
        //타이틀 이미지가 일정 높이 까지 올라 온 뒤 타이틀이 배치됐음을 알림
        if (titleImage.transform.position.y >= 450f)
        {
            button.SetActive(true);
            return;
        }
        else
        {
            titleMovePosition_Y += titleMoveSpeed;
        }
        
        titleImage.transform.Translate(0, titleMovePosition_Y, 0);
    }

    public void ClickStatButton()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void ClickCreditButton()
    {
        creditUI.SetActive(true);
    }

    public void ClickHelpButton()
    {
        if (creditUI.activeSelf != true)
        {
            descriptionUI.SetActive(true);  
        }
    }

    public void ClickCloseButton()
    {
        if (creditUI.gameObject.activeSelf == true)
        {
            creditUI.SetActive(false);
            helpButton.SetActive(true);
        }
        else if(descriptionUI.gameObject.activeSelf == true)
        {
            descriptionUI.SetActive(false);
        }
    }

    public void ClickRankingButton()
    {
        SceneManager.LoadScene("Ranking");
    }

    public void ClickExitButton()
    {
        Application.Quit();
    }
}



