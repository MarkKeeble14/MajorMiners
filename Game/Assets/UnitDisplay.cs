using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void Set(GameObject spawned)
    {
        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
        BaseUnit unit = spawned.GetComponent<BaseUnit>();
        cost.text = "$" + unit.Cost.ToString();
        nameDisplay.text = unit.name;
        sprite.sprite = sr.sprite;
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
