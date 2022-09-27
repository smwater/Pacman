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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "Stage")
            {  
                SceneManager.LoadScene("Title");
                return;
            }
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 게임 오버 후 또는 랭킹창을 열었을 경우 점수 갱신
    /// 이벤트로 한번 호출하도록 해야함
    /// </summary>
    private void UpdateRanking()
    {
        //1~3위

        //4~10위
    }
}
