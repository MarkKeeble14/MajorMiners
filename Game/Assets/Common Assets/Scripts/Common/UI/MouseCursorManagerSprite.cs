using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType
{
    DEFAULT,
    TARGET
}

public class MouseCursorManagerSprite : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private List<SerializableKeyValuePair<CursorType, Sprite>> cursorType = new List<SerializableKeyValuePair<CursorType, Sprite>>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        sr = GetComponent<SpriteRenderer>();
        SetCursorType(CursorType.DEFAULT);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
    }

    public void SetCursorType(CursorType type)
    {
        sr.sprite = GetSpriteFromCursorType(type);
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
