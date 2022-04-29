using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Set(string text, Color c)
    {
        this.text.text = text;
        this.text.color = c;
    }

    public void Set(string text)
    {
        this.text.text = text;
    }
}
