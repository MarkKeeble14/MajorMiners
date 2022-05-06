using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGameController : MonoBehaviour
{
    [SerializeField] private GameObject gameOnUI;
    [SerializeField] private GameObject preGameUI;
    [SerializeField] private GridTileManager gTileManager;
    [SerializeField] private GameObject GameManager;
    [SerializeField] private GameObject defaultCamera;
    [SerializeField] private TimerReadFrom timer;
    [SerializeField] private ResultsScreenController resultsScreen;
    [SerializeField] private AttackerPlayer attacker;
    [SerializeField] private DefenderPlayer defender;
    [SerializeField] private UIButtonHelper buttonHelper;

    public void StartGame()
    {
        buttonHelper.UnpauseGame();
        resultsScreen.Close();
        timer.Reset();

        preGameUI.SetActive(false);
        defaultCamera.SetActive(false);
        gameOnUI.SetActive(true);

        gTileManager.Spawn();
        defender.ResetPlayer();
        attacker.ResetPlayer();
        GameManager.SetActive(true);
    }

    public void OpenPreGameUI()
    {
        timer.Reset();
        resultsScreen.Close();

        gameOnUI.SetActive(false);
        GameManager.SetActive(false);
        preGameUI.SetActive(true);

        attacker.DestroyAllSpawns();
        defender.DestroyAllSpawns();
        
        NumberOptionSetter[] arr = FindObjectsOfType<NumberOptionSetter>();
        foreach (NumberOptionSetter os in arr)
        {
            os.SetLabels();
        }

        defaultCamera.SetActive(true);
    }
}
