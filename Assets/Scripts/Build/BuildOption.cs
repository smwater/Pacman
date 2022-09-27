using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOption : MonoBehaviour
{
    private void Start()
    {
        SetResolution();
    }

    /// <summary>
    /// ���� ������ �ػ󵵸� �����Ѵ�.
    /// </summary>
    private void SetResolution()
    {
        int setWidth = 500;
        int setHeight = 500;

        Screen.SetResolution(setWidth, setHeight, false);
    }
}
