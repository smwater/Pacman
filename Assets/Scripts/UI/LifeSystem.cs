using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    private int _heartCount;
    private GameObject[] _hearts;

    private int _lifeCount = 4;

    private void Awake()
    {
        _heartCount = transform.childCount;
        _hearts = new GameObject[_heartCount];
        for (int i = 0; i < _heartCount; i++)
        {
            _hearts[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.PlayerDead.AddListener(EraseLife);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerDead.RemoveListener(EraseLife);
    }

    /// <summary>
    /// 오른쪽 라이프부터 차례대로 지운다.
    /// </summary>
    private void EraseLife()
    {
        if (_lifeCount <= 0)
        {
            return;
        }

        _hearts[_lifeCount - 1].GetComponent<Life>().Erase();

        _lifeCount--;
    }
}
