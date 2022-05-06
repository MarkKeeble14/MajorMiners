using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurrenderButton : MonoBehaviour
{
    [SerializeField] private ResultsScreenController resultsScreen;
    private VICTORY_METHOD method = VICTORY_METHOD.SURRENDER;
    public bool isAttacker;

    public void Surrender()
    {
        if (isAttacker)
        {
            resultsScreen.DefenderWon(method);
        } else
        {
            resultsScreen.AttackerWon(method);
        }
    }
}
