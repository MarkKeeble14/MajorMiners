using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player attacker;

    private void Update()
    {
        attacker.Update();
    }
}

[System.Serializable]
public class Player
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

        if (!placing)
            return;
        ControlCursor();
    }

    private void ControlCursor()
    {
        if (Input.GetKeyDown(moveCursorLeft))
        {
            tileCursor.Move(-1, 0);
        }
        if (Input.GetKeyDown(moveCursorRight))
        {
            tileCursor.Move(1, 0);
        }
        if (Input.GetKeyDown(moveCursorDown))
        {
            tileCursor.Move(0, 1);
        }
        if (Input.GetKeyDown(moveCursorUp))
        {
            tileCursor.Move(0, -1);
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



[System.Serializable]
public class ShopUnit
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int cost;
    public int Cost
    {
        get { return cost; }
    }
    [SerializeField] private PlacementType[] placementType;
}

public enum PlacementType
{
    WALKABLE_TILE,
    BLOCKED_TILE
}