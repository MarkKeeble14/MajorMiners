using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;
using UnityEngine.UI;
using System.Collections;

public class Vector2OptionSetter : OptionSetter
{
    [Header("References")]
    [SerializeField] private TMP_InputField x;
    [SerializeField] private TMP_InputField y;
    [SerializeField] private TextMeshProUGUI xPlaceHolderTMP;
    [SerializeField] private TextMeshProUGUI yPlaceHolderTMP;
    [SerializeField] private Vector2ReadFrom rf;

    [Header("Settings")]
    [SerializeField] private string xPlaceHolder = "Enter text...";
    [SerializeField] private string yPlaceHolder = "Enter text...";
    [SerializeField] private HasBoundary xMin;
    [SerializeField] private HasBoundary xMax;
    [SerializeField] private HasBoundary yMin;
    [SerializeField] private HasBoundary yMax;

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
        SetLabels();
        SetFields();
        SetBounds();
    }

    private void SetLabels()
    {
        labelTMP.text = label;
        xPlaceHolderTMP.text = xPlaceHolder;
        yPlaceHolderTMP.text = yPlaceHolder;
    }

    private void SetFields()
    {
        x.text = StringHelper.ToDetailedString(rf.Value.x);
        y.text = StringHelper.ToDetailedString(rf.Value.y);
    }

    private void SetBounds()
    {
        if (xMax.Has && !yMin.Has)
        {
            yMin.Has = true;
            yMin = xMax;
        }
        if (yMin.Has && xMax.Has)
        {
            xMax.Has = true;
            xMax = yMin;
        }
    }

    public void SetX(string s)
    {
        s = s.Replace(',', '.');
        float number;
        if (!float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            if (s.Length == 0)
                return;
            x.text = StringHelper.ToDetailedString(rf.Value.x);
            InitErrorMessage("Couldn't parse number", errorDuration);
            return;
        }
        if (xMin.Has && number < xMin.Bound)
        {
            x.text = StringHelper.ToDetailedString(rf.Value.x);
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number) 
                + " Too Small, Must be Larger than: " + StringHelper.ToDetailedString(xMin.Bound), errorDuration);
            return;
        }
        if (xMax.Has && number > xMax.Bound)
        {
            x.text = StringHelper.ToDetailedString(rf.Value.x);
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
                + " Too Large, Must be Smaller than: " + StringHelper.ToDetailedString(xMax.Bound), errorDuration);
            return;
        }
        rf.Value = new Vector2(number, rf.Value.y);
        if (rf.Value.x > rf.Value.y)
        {
            rf.Value = new Vector2(number, number);
            SetFields();
        }
    }

    public void SetY(string s)
    {
        s = s.Replace(',', '.');
        float number;
        if (!float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            if (s.Length == 0)
                return;
            y.text = StringHelper.ToDetailedString(rf.Value.y);
            InitErrorMessage("Couldn't parse number", errorDuration);
            return;
        }
        if (yMin.Has && number < yMin.Bound)
        {
            y.text = StringHelper.ToDetailedString(rf.Value.y);
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
                + " Too Large, Must be Smaller than: " + StringHelper.ToDetailedString(yMin.Bound), errorDuration);
            return;
        }
        if (yMax.Has && number > yMax.Bound)
        {
            y.text = StringHelper.ToDetailedString(rf.Value.y);
            InitErrorMessage("Given number: " + StringHelper.ToDetailedString(number)
                + " Too Large, Must be Smaller than: " + StringHelper.ToDetailedString(yMax.Bound), errorDuration);
            return;
        }
        rf.Value = new Vector2(rf.Value.x, number);
        if (rf.Value.y < rf.Value.x)
        {
            rf.Value = new Vector2(number, number);
            SetFields();
        }
    }
}