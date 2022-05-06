using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;

public class NumberOptionSetter : OptionSetter
{
    [Header("References")]
    [SerializeField] private TMP_InputField num;
    [SerializeField] private TextMeshProUGUI placeHolderTMP;
    [SerializeField] private NumReadFrom rf;

    [Header("Settings")]
    [SerializeField] private string placeHolder = "Enter text...";
    [SerializeField] private HasBoundary numMin;
    [SerializeField] private HasBoundary numMax;

    [SerializeField] protected bool oddNumbersOnly;

    private void OnEnable()
    {
        Set();
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void Start()
    {
        Set();
    }

    protected override void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        SetLabels();
    }

    private void Set()
    {
        SetLabels();
        SetNum();
    }

    public void SetLabels()
    {
        labelTMP.text = label;
        placeHolderTMP.text = placeHolder;
    }

    private void SetNum()
    {
        num.text = StringHelper.ToDetailedString(rf.Value);
    }

    public void SetNum(string s)
    {
        s = s.Replace(',', '.');
        float number;
        if (!float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            if (s.Length == 0)
                return;
            num.text = StringHelper.ToDetailedString(rf.Value);
            return;
        }
        if (numMin.Has && number < numMin.Bound)
        {
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
    + " Too Small, Must be Larger than: " + StringHelper.ToDetailedString(numMin.Bound), errorDuration);
            num.text = StringHelper.ToDetailedString(rf.Value);
            return;
        }
        if (numMax.Has && number > numMax.Bound)
        {
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
    + " Too Large, Must be Smaller than: " + StringHelper.ToDetailedString(numMax.Bound), errorDuration);
            num.text = StringHelper.ToDetailedString(rf.Value);
            return;
        }
        if (oddNumbersOnly && number % 2 == 0)
        {
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
    + " is Even, Converting to Odd due to Game Rules: " + StringHelper.ToDetailedString(++number), errorDuration);
            rf.Value = number;
            num.text = StringHelper.ToDetailedString(rf.Value);
            return;
        } else
        {
            if (rf is TimerReadFrom)
                ((TimerReadFrom)rf).Value = number;
            else
                rf.Value = number;
        }
    }
}
