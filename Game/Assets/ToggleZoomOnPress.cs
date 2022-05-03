using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleZoomOnPress : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private float camSizeMin = 10f;
    [SerializeField] private float camSizeMax = 18f;
    [SerializeField] private float speed = 5f;
    private float targetSize;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetSize = camSizeMin;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (targetSize == camSizeMin)
            {
                targetSize = camSizeMax;
            } else
            {
                targetSize = camSizeMin;
            }
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * speed);
    }
}
