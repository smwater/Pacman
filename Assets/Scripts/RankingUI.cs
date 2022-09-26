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
    /// RankingUI �˾�â�� ���� �޼ҵ�
    /// �����̽��ٸ� ���� ��� Title �������� �ڽ��� ����, ���ӿ��� �� stage�������� Title���� �ҷ���
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
    /// ���� ���� �� �Ǵ� ��ŷâ�� ������ ��� ���� ����
    /// </summary>
    private void UpdateRanking()
    {
        //1~3��

        //4~10��
    }
}
