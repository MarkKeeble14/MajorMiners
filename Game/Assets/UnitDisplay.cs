using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}

public enum LeftOrRight
{
    LEFT,
    RIGHT
}

public class UnitDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private Image sprite;
    [SerializeField] private Image background;
    [SerializeField] private RadialBarDisplay rBar;
    public bool selected = false;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color notSelectedColor;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TextMeshProUGUI tooltipText;

    public void Set(GameObject spawned)
    {
        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
        BaseUnit unit = spawned.GetComponent<BaseUnit>();
        cost.text = "$" + unit.Cost.ToString();
        tooltipText.text = unit.Description;
        nameDisplay.text = unit.name;
        sprite.sprite = sr.sprite;
    }

    public void SetLean(LeftOrRight lean)
    {
        RectTransform rt = tooltip.GetComponent<RectTransform>();
        switch (lean)
        {
            case LeftOrRight.LEFT:
                RectTransformExtensions.SetLeft(rt, -120);
                break;
            case LeftOrRight.RIGHT:
                RectTransformExtensions.SetRight(rt, -120);
                break;
        }
    }

    public void ToggleTooltip()
    {
        if (tooltip.activeSelf)
        {
            CloseTooltip();
        }
        else
        {
            OpenTooltip();
        }
    }

    public void OpenTooltip()
    {
        // Can Animate later on
        tooltip.SetActive(true);
    }

    public void CloseTooltip()
    {
        tooltip.SetActive(false);
    }

    public void SetCD(float cur, float max)
    {
        rBar.SetSize(cur, max);
    }

    private void Update()
    {
        background.color = selected ? selectedColor : notSelectedColor;
    }
}
