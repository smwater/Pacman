using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RankingUI : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name != "Title")
            {
                SceneManager.LoadScene("Title");
            }
            gameObject.SetActive(false);
        }
    }
}
