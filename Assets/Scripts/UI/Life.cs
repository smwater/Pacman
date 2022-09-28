using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    /// <summary>
    /// 라이프 하나를 지운다.
    /// </summary>
    public void Erase()
    {
        gameObject.SetActive(false);
    }
}
