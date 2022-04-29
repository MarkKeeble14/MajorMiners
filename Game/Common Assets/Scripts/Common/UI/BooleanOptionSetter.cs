using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

public class BooleanOptionSetter : OptionSetter
{
    [Header("References")]
    [SerializeField] private TMP_InputField num;
    [SerializeField] private TextMeshProUGUI placeHolderTMP;
    [SerializeField] private Toggle toggle;
    [SerializeField] private BoolLockedNumReadFrom rf;

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
        Set();
    }

    private void Set()
    {
        SetLabel();
        SetValue();
    }

    private void SetLabel()
    {
        labelTMP.text = label;
    }

    private void SetValue()
    {
        num.text = StringHelper.ToDetailedString(rf.Value);
        toggle.isOn = rf.Bool;
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
        rf.Value = number;
    }

    public void SetBool(bool b)
    {
        rf.Bool = b;
    }
}
