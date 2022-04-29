using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumReadFromController : MonoBehaviour
{
    [SerializeField] private string label;
    [SerializeField] private NumReadFrom rf;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!label.Equals(""))
            text.text = label + ": " + rf.Value;
        else
            text.text = rf.Value.ToString();
    }
}
