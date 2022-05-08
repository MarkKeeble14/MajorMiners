using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class ResultsScreenController : MonoBehaviour
{
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private TextMeshProUGUI methodText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private List<SerializableKeyValuePair<VICTORY_METHOD, string>> methodStrings = new List<SerializableKeyValuePair<VICTORY_METHOD, string>>();
    private bool open;

    private string FindString(VICTORY_METHOD m)
    {
        foreach (SerializableKeyValuePair<VICTORY_METHOD, string> s in methodStrings)
        {
            if (m == s.Key)
            {
                return s.Value;
            }
        }
        return null;
    }

    public void AttackerWon(VICTORY_METHOD method)
    {
        if (open) return;
        Open();
        winnerText.text = "The Attacker Won!";
        methodText.text = FindString(method);
    }

    public void DefenderWon(VICTORY_METHOD method)
    {
        if (open) return;
        Open();
        winnerText.text = "The Defender Won!";
        methodText.text = FindString(method);
    }

    private void Open()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Human_Cheer");
        resultsScreen.SetActive(true);
        open = true;
    }

    public void Close()
    {
        resultsScreen.SetActive(false);
        open = false;
    }
}
