using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    /// <summary>
    /// ������ �ϳ��� �����.
    /// </summary>
    public void Erase()
    {
        gameObject.SetActive(false);
    }
}
