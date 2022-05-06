using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static CoolerAudioManager audioManager;
    public Player attacker;
    public Player defender;

    public static int numberOfUnits;

    private void Awake()
    {
        audioManager = FindObjectOfType<CoolerAudioManager>();
        if (!audioManager) return;
        audioManager.gameStartParam = 1;
    }

    private void Update()
    {
        attacker.UpdatePlayer();
        defender.UpdatePlayer();
    }

    public static void IncreaseUnits()
    {
        ++numberOfUnits;

        CheckIntensity();
    }

    public static void DecreaseUnits()
    {
        --numberOfUnits;
        
        CheckIntensity();
    }

    public static void HurryUp()
    {
        if (!audioManager) return;
        audioManager.hurryUpParam = 1;
    }

    private static void CheckIntensity()
    {
        if (!audioManager) return;
        if (numberOfUnits < 5)
        {
            audioManager.intensityLevel = 0;
        }
        else if (numberOfUnits >= 5 && numberOfUnits < 10)
        {
            audioManager.intensityLevel = 1;
        }
        else if (numberOfUnits >= 10 && numberOfUnits < 15)
        {
            audioManager.intensityLevel = 2;
        }
        else
        {
            audioManager.intensityLevel = 3;
        }
    }
}
