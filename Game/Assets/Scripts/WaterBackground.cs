using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBackground : MonoBehaviour
{
    [SerializeField] private NumReadFrom rows;
    [SerializeField] private NumReadFrom columns;
    [SerializeField] int extraSpace;

    // Update is called once per frame
    void Update()
    {
        float size = Mathf.Max(rows.Value + extraSpace, columns.Value + extraSpace) * 4;
        transform.localScale = new Vector3(size, size, 1);
    }
}
