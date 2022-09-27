using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RankingUI : MonoBehaviour
{
    void Update()
    {
        UpdateRanking();
        CloseRankingUI();
    }

    /// <summary>
    /// RankingUI 팝업창을 끄는 메소드
    /// 스페이스바를 누를 경우 Title 씬에서는 자신을 끄고, 게임오버 후 stage씬에서는 Title씬을 불러옴
    /// </summary>
    private void CloseRankingUI()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                gameObject.SetActive(false);
            }
            else if(SceneManager.GetActiveScene().name == "Ranking")
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    /// <summary>
    /// 게임 오버 후 또는 랭킹창을 열었을 경우 점수 갱신
    /// </summary>
    private void UpdateRanking()
    {
        //1~3위

        //4~10위
    }
}
