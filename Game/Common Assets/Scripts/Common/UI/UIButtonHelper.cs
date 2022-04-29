using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHelper : MonoBehaviour
{
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += ResetScene;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePauseGame()
    {
        if (paused)
        {
            paused = false;
            Time.timeScale = 1;
        } else
        {
            paused = true;
            Time.timeScale = 0;
        }
    }

    private void ResetScene(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
    }
}
