using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{

    [SerializeField]
    float titleMoveSpeed;
    
    GameObject titleImage;

    float titleMovePosition_Y;
    bool isSetTitle;

    GameObject GameStartButton;
    GameObject CreditButton;
    GameObject HelpButton;
    
    GameObject DescriptionUI;
    GameObject CreditUI;


    private void Awake()
    {
        isSetTitle = false;

        titleImage = GameObject.Find("TitileImage");
        
        GameStartButton = GameObject.Find("StartButton");
        CreditButton = GameObject.Find("CreditButton");
        HelpButton = GameObject.Find("?_Button");
        
        DescriptionUI = GameObject.Find("DescriptionUI");
        CreditUI = GameObject.Find("CreditUI");
        
    }

    void Start()
    {
        GameStartButton.SetActive(false);
        CreditButton.SetActive(false);
        HelpButton.SetActive(false);
        
        DescriptionUI.SetActive(false);
        CreditUI.SetActive(false);
        
    }

    void Update()
    {
        ShowButton(isSetTitle);
        TitleMove();
    }

    private void TitleMove()
    {
        if (titleImage.transform.position.y >= 500f)
        {
            isSetTitle = true;
            return;
        }
        else
        {
            titleMovePosition_Y += titleMoveSpeed;
        }
        
        titleImage.transform.Translate(0, titleMovePosition_Y, 0);
    }

    private void ShowButton(bool isSet)
    {
        if (isSet == true)
        {
            GameStartButton.SetActive(true);
            CreditButton.SetActive(true);
            HelpButton.SetActive(true);
        }
    }

    public void ClickStatButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void ClickCreditButton()
    {
        CreditUI.SetActive(true);
        HelpButton.SetActive(false);
    }

    public void ClickHelpButton()
    {
        if (CreditUI.activeSelf != true)
        {
            DescriptionUI.SetActive(true);  
        }
    }

    public void ClickExitButton()
    {
        if (CreditUI.gameObject.activeSelf == true)
        {
            CreditUI.SetActive(false);
            HelpButton.SetActive(true);
        }
        else if(DescriptionUI.gameObject.activeSelf == true)
        {
            DescriptionUI.SetActive(false);
        }
    }
}



