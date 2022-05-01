using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Player : MonoBehaviour
{
    public int money;
    public bool placing;
    
    [SerializeField] protected TileCursor tileCursor;
    [SerializeField] private KeyCode enterPlacementMode = KeyCode.LeftShift;
    [SerializeField] private KeyCode exitPlacementMode = KeyCode.Escape;
    [SerializeField] protected KeyCode moveCursorLeft = KeyCode.LeftArrow;
    [SerializeField] protected KeyCode moveCursorRight = KeyCode.RightArrow;
    [SerializeField] protected KeyCode moveCursorUp = KeyCode.UpArrow;
    [SerializeField] protected KeyCode moveCursorDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode _placeUnit = KeyCode.Tab;
    [SerializeField] protected GameObject[] _unitsToSpawn;

    protected int currentUnitIndex;

    private void Update()
    {
        if (Input.GetKeyDown(enterPlacementMode))
            OpenPlacementMode();

        if (Input.GetKeyDown(exitPlacementMode))
            ClosePlacementMode();
        
        if (!placing)
        {
            ControlUnitCursor();
        }
        else
        {
            ControlPlacementCursor();
        }
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

    private void ControlUnitCursor()
    {
        if (Input.GetKeyDown(moveCursorLeft))
        {
            --currentUnitIndex;
            if (currentUnitIndex < 0)
            {
                currentUnitIndex = _unitsToSpawn.Length - 1;
            }
            
            Debug.Log(_unitsToSpawn[currentUnitIndex]);
        }

        if (Input.GetKeyDown(moveCursorRight))
        {
            ++currentUnitIndex;
            if (currentUnitIndex >= _unitsToSpawn.Length)
            {
                currentUnitIndex = 0;
            }
            
            Debug.Log(_unitsToSpawn[currentUnitIndex]);
        }
    }

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