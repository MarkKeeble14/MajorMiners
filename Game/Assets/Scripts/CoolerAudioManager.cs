using FMODUnity;
using UnityEngine;

public class CoolerAudioManager : MonoBehaviour
{
    [SerializeField]
    [FMODUnity.EventRef]
    private string aSound;

    public FMOD.Studio.EventInstance mainAudio;
    public int gameStartParam = 0;
    public int intensityLevel = 0;
    public int hurryUpParam = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mainAudio = RuntimeManager.CreateInstance(aSound);
        mainAudio.start();
    }

    private void Update()
    {
        mainAudio.setParameterByName("GameStart", gameStartParam);
        mainAudio.setParameterByName("Intensity", intensityLevel);
        mainAudio.setParameterByName("HurryUp", hurryUpParam);
    }

    private void OnDestroy()
    {
        mainAudio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        mainAudio.release();
    }
}
