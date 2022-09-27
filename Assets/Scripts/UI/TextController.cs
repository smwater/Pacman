using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI _text;

    public void On()
    {
        _text.gameObject.SetActive(true);
    }

    public void Off()
    {
        _text.gameObject.SetActive(false);
    }
}
