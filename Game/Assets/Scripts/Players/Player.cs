using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Player : MonoBehaviour
{
    public uint money;
    public bool placing;

    [SerializeField] protected TileCursor tileCursor;
    [SerializeField] private KeyCode[] _unitKeys;
    [SerializeField] private string[] _buttonNames;

    [SerializeField] private KeyCode exitPlacementMode = KeyCode.Backspace;

    [SerializeField] protected KeyCode moveCursorLeft = KeyCode.LeftArrow;
    [SerializeField] protected KeyCode moveCursorRight = KeyCode.RightArrow;
    [SerializeField] protected KeyCode moveCursorUp = KeyCode.UpArrow;
    [SerializeField] protected KeyCode moveCursorDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode _placeUnit = KeyCode.Return;
    [SerializeField] protected GameObject[] _unitsToSpawn;

    protected int currentUnitIndex;

    private void Update()
    {
        for (var i = 0; i < _unitKeys.Length; ++i)
        {
            //if (!!Input.GetKeyDown(_unitKeys[i]) && !Input.GetButtonDown(_buttonNames[0])) continue;
            if (!Input.GetKeyDown(_unitKeys[i])) continue;


            currentUnitIndex = i;
            break;
        }
        
        ControlPlacementCursor();
    }

    private void ControlPlacementCursor()
    {
        if (!Input.GetKey(moveCursorLeft)
            && !Input.GetKey(moveCursorRight)
            && !Input.GetKey(moveCursorUp)
            && !Input.GetKey(moveCursorDown))
        {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(moveCursorLeft))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorLeft, -1, 0, 0.5f, 0.05f));
        }

        if (Input.GetKeyDown(moveCursorRight))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorRight, 1, 0, 0.5f, 0.05f));
        }

        if (Input.GetKeyDown(moveCursorUp))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorUp, 0, -1, 0.5f, 0.05f));
        }

        if (Input.GetKeyDown(moveCursorDown))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorDown, 0, 1, 0.5f, 0.05f));
        }

        // Place unit down.
        if (Input.GetKeyDown(_placeUnit))
        {
            TryPlaceUnit();
        }
    }

    protected abstract void TryPlaceUnit();

    private IEnumerator StartMove(KeyCode key, int moveRow, int moveCol,
        float waitAfterFirstMove,
        float waitPastFirstMove)
    {
        bool first = true;
        tileCursor.Move(moveRow, moveCol);
        while (Input.GetKey(key))
        {
            if (first)
            {
                yield return new WaitForSeconds(waitAfterFirstMove);
                first = false;
            }
            else
            {
                yield return new WaitForSeconds(waitPastFirstMove);
            }

            tileCursor.Move(moveRow, moveCol);
            yield return null;
        }
    }

    private void OpenPlacementMode()
    {
        placing = true;
        tileCursor.Show();
    }

    protected void ClosePlacementMode()
    {
        placing = false;
        tileCursor.Hide();
    }

    public void OpenMultiPlacementMode(ShopUnit unit, int number)
    {
        // Attacker does not have enough money to place the units
        if (money < unit.Cost * number)
            return;
        OpenPlacementMode();
    }

    public void OpenSinglePlacementMode(ShopUnit unit)
    {
        // Attacker does not have enough money to place the unit
        if (money < unit.Cost)
            return;
        OpenPlacementMode();
    }
}