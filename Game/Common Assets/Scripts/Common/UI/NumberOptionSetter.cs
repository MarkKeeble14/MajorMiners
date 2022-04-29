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

    private void OnEnable()
    {
        Set();
        SceneManager.sceneLoaded += OnSceneChange;
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

    private void SetLabels()
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
            num.text = StringHelper.ToDetailedString(rf.Value);
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
    + " Too Small, Must be Larger than: " + StringHelper.ToDetailedString(numMin.Bound), errorDuration);
            return;
        }
        if (numMax.Has && number > numMax.Bound)
        {
            num.text = StringHelper.ToDetailedString(rf.Value);
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
    + " Too Large, Must be Smaller than: " + StringHelper.ToDetailedString(numMax.Bound), errorDuration);
            return;
        }
        if (rf is TimerReadFrom)
            ((TimerReadFrom)rf).Value = number;
        else
            rf.Value = number;
    }
}
