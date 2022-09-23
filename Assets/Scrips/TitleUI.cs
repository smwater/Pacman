using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUI : MonoBehaviour
{
    

    [SerializeField]
    private float _titleSpeed;
    [SerializeField]
    private float _stopPosition;

    private GameObject _titleImage;
    private float _moveTitlePositionY;
    private GameObject _buttons;
    private GameObject _helpButton;
    
    //팝업 창
    private GameObject _descriptionUI;
    private GameObject _creditUI;

    private void Awake()
    {
        _titleImage = GameObject.Find("TitileImage");
        _buttons = GameObject.Find("Buttons");
        _helpButton = GameObject.Find("HelpButton");
        
        _descriptionUI = GameObject.Find("DescriptionUI");
        _creditUI = GameObject.Find("CreditUI");
    }

    void Start()
    {
        _buttons.SetActive(false);
        _descriptionUI.SetActive(false);
        _creditUI.SetActive(false);
    }

    void Update()
    {
        MoveTitle();
    }

    private void MoveTitle()
    {
        //타이틀 이미지가 일정 높이 까지 올라 온 뒤 타이틀이 배치됐음을 알림
        if (_titleImage.transform.position.y >= _stopPosition)
        {
            _buttons.SetActive(true);
            return;
        }
        else
        {
            _moveTitlePositionY += _titleSpeed;
        }
        
        _titleImage.transform.Translate(0, _moveTitlePositionY, 0);
    }

    public void ClickStatButton()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void ClickCreditButton()
    {
        _creditUI.SetActive(true);
    }

    public void ClickHelpButton()
    {
        if (_creditUI.activeSelf != true)
        {
            _descriptionUI.SetActive(true);  
        }
    }

    public void ClickCloseButton()
    {
        if (_creditUI.gameObject.activeSelf == true)
        {
            _creditUI.SetActive(false);
            _helpButton.SetActive(true);
        }
        else if(_descriptionUI.gameObject.activeSelf == true)
        {
            _descriptionUI.SetActive(false);
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



