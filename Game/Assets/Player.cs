using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    public int money;
    public bool placing;

    [SerializeField] private TileCursor tileCursor;
    [SerializeField] private KeyCode enterPlacementMode = KeyCode.KeypadEnter;
    [SerializeField] private KeyCode exitPlacementMode = KeyCode.Escape;
    [SerializeField] private KeyCode moveCursorLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode moveCursorRight = KeyCode.RightArrow;
    [SerializeField] private KeyCode moveCursorUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode moveCursorDown = KeyCode.DownArrow;

    public void Update()
    {
        if (Input.GetKeyDown(enterPlacementMode))
            OpenPlacementMode();

        if (Input.GetKeyDown(exitPlacementMode))
            ClosePlacementMode();

        if (Input.GetKeyDown(KeyCode.R))
            tileCursor.SetBreakable(true);

        if (!placing)
            return;
        ControlCursor();
    }

    private void ControlCursor()
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

    private void ClosePlacementMode()
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