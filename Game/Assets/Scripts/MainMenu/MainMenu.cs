using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CoolerAudioManager audioManager;
    
    public void OnPlay()
    {
        
        Debug.Log("HERE");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
