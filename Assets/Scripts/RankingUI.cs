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
    /// ���� ���� �� �Ǵ� ��ŷâ�� ������ ��� ���� ����
    /// �̺�Ʈ�� �ѹ� ȣ���ϵ��� �ؾ���
    /// </summary>
    private void UpdateRanking()
    {
        //1~3��

        //4~10��
    }
}
