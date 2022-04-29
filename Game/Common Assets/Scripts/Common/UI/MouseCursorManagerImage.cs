using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursorManagerImage : MonoBehaviour
{
    private Image image;

    [SerializeField] private List<SerializableKeyValuePair<CursorType, Sprite>> cursorType = new List<SerializableKeyValuePair<CursorType, Sprite>>();

    new private RectTransform transform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 sizeOffset;

    public bool enabled;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        transform = GetComponent<RectTransform>();
        SetCursorType(CursorType.DEFAULT);
        if (!enabled)
            return;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled)
        {
            image.enabled = false;
            return;
        }
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        pos += new Vector2(transform.sizeDelta.x, -transform.sizeDelta.y) + sizeOffset;
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void SetCursorType(CursorType type)
    {
        image.sprite = GetSpriteFromCursorType(type);
    }

    private Sprite GetSpriteFromCursorType(CursorType type)
    {
        foreach (SerializableKeyValuePair<CursorType, Sprite> kvp in cursorType)
        {
            if (kvp.Key == type)
                return kvp.Value;
        }
        return null;
    }
}