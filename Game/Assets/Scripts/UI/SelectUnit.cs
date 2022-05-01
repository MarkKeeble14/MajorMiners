using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectUnit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int currentIndex;
    [SerializeField] private List<GameObject> units;
    [SerializeField] private GameObject display;

    private void Start()
    {
        var obj = GameObject.Instantiate(display);
        obj.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown())
    }
}
