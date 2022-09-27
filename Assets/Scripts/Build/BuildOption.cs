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
    /// 빌드 파일의 해상도를 고정한다.
    /// </summary>
    private void SetResolution()
    {
        int setWidth = 500;
        int setHeight = 500;

        Screen.SetResolution(setWidth, setHeight, false);
    }
}
