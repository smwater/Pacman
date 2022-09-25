using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    private int SCORE_NUMBER = 10;
    private int TMP_SCORE = 0;
    private int[] _bestScore = new int[10];

    //�÷��̾� ���ھ ���� ���� ������ ���ĵǴ� �ڵ�
    void ScoreSet(int currentScore)
    {
        PlayerPrefs.SetInt("CurrentPlayerScore", currentScore);

        for(int i = 0; i < SCORE_NUMBER; i++)
        {
            _bestScore[i] = PlayerPrefs.GetInt(i + "BestScore");
            
            while(_bestScore[i] < currentScore)
            {
                TMP_SCORE = _bestScore[i];
                _bestScore[i] = currentScore;

                PlayerPrefs.SetInt(i + "BestScore", currentScore);

                currentScore = TMP_SCORE;
            }
        }

        for(int i = 0; i < SCORE_NUMBER; i++)
        {
            PlayerPrefs.SetInt(i + "BestScore", _bestScore[i]);
        }

    }


}
