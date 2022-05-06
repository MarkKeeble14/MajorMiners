using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Image sprite;
    [SerializeField] private Image background;
    [SerializeField] private RadialBarDisplay rBar;
    public bool selected = false;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color notSelectedColor;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TextMeshProUGUI tooltipName;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private TextMeshProUGUI selectKey;
    [SerializeField] private int size = 125;
    [SerializeField] private bool open;
    public bool IsOpen
    {
        get { return open; }
    }
    public void Set(GameObject spawned, int num)
    {
        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
        BaseUnit unit = spawned.GetComponent<BaseUnit>();
        cost.text = "$" + unit.Cost.ToString();
        tooltipName.text = unit.name;
        tooltipText.text = unit.Description;
        selectKey.text = num.ToString();
        sprite.sprite = sr.sprite;
    }

    public void SetLean(LeftOrRight lean)
    {
        RectTransform rt = tooltip.GetComponent<RectTransform>();
        switch (lean)
        {
            case LeftOrRight.LEFT:
                RectTransformExtensions.SetLeft(rt, -size);
                break;
            case LeftOrRight.RIGHT:
                RectTransformExtensions.SetRight(rt, -size);
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
        tooltip.SetActive(true);
        open = true;
    }

    public void CloseTooltip()
    {
        tooltip.SetActive(false);
        open = false;
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
