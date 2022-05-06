using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionsScreen;

    public void OpenOptionsScreen()
    {
        CloseAllScreens();
        optionsScreen.SetActive(true);
    }

    public void OpenMainScreen()
    {
        CloseAllScreens();
        mainScreen.SetActive(true);
    }

    public void CloseAllScreens()
    {
        mainScreen.SetActive(false);
        optionsScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
