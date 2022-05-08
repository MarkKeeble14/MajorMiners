using FMODUnity;
using System.Collections;
using UnityEngine;

public class CoolerAudioManager : MonoBehaviour
{
    private static CoolerAudioManager _instance;

    public static CoolerAudioManager Instance { get { return _instance; } }
    [SerializeField]
    [FMODUnity.EventRef]
    private string aSound;
    [SerializeField] private string masterBank;
    public FMOD.Studio.EventInstance mainAudio;
    public int gameStartParam = 0;
    public int intensityLevel = 0;
    public int hurryUpParam = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(LoadMusic());
        }
    }

    private IEnumerator LoadMusic()
    {
        Debug.Log("Start Load Music");
        RuntimeManager.LoadBank(masterBank, true);

        while (!RuntimeManager.HasBankLoaded(masterBank))
        {
            Debug.Log("Hasn't Loaded Bank: " + masterBank);
            yield return null;
        }
        Debug.Log("Loaded Bank: " + masterBank);

        StartMusic();
    }

    private void StartMusic()
    {
        Debug.Log("Starting Music");
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
