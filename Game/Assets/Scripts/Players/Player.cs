using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Grid;
using UI;
using UnityEngine;

[System.Serializable]
public abstract class Player : MonoBehaviour
{
    public uint money;
    public bool placing;

    [SerializeField] protected TileManager tileManager;
    [SerializeField] private SpriteRenderer selectedIndicator;
    [SerializeField] protected TileCursor tileCursor;
    [SerializeField] private GameObject buyableUnitDisplay;
    [SerializeField] private Transform buyableUnitDisplayParent;
    [SerializeField] private string _placeButtonName;
    [SerializeField] private string[] _buttonNames;
    [SerializeField] private string horizontalString = "Horizontal";
    [SerializeField] private string verticalString = "Vertical";

    [SerializeField] protected GameObject[] _unitsToSpawn;
    [SerializeField] private KeyCode[] _unitKeys;
    [SerializeField] private KeyCode moveCursorLeft;
    [SerializeField] private KeyCode moveCursorRight;
    [SerializeField] private KeyCode moveCursorDown;
    [SerializeField] private KeyCode moveCursorUp;
    [SerializeField] private KeyCode _placeUnit = KeyCode.Return;
    
    [SerializeField] private float _delayAfterKeyPressed = 0.5f;
    [SerializeField] private float _delayWhileKeyHeld = 0.05f;

    protected Dictionary<GameObject, BaseUnit> baseUnitDictionary = new Dictionary<GameObject, BaseUnit>();
    protected Dictionary<GameObject, UnitDisplay> unitDisplayDictionary = new Dictionary<GameObject, UnitDisplay>();
    protected Dictionary<GameObject, Timer> cooldowns = new Dictionary<GameObject, Timer>();
    private UnitDisplay[] unitDisplays;

    protected int currentUnitIndex;
    protected bool placedUnit;

    [SerializeField] protected List<GameObject> spawnedUnits = new List<GameObject>();

    private void Awake()
    {
        selectedIndicator.sprite = _unitsToSpawn[currentUnitIndex].GetComponent<SpriteRenderer>().sprite;

        unitDisplays = new UnitDisplay[_unitsToSpawn.Length];
        for (int i = 0; i < _unitsToSpawn.Length; i++)
        {
            // father forgive me for i have sinned
            GameObject o = _unitsToSpawn[i];
            BaseUnit bUnit = o.GetComponent<BaseUnit>();
            baseUnitDictionary.Add(o, bUnit);
            GameObject spawned = Instantiate(buyableUnitDisplay, buyableUnitDisplayParent);
            UnitDisplay sUnitDisplay = spawned.GetComponent<UnitDisplay>();
            unitDisplays[i] = sUnitDisplay;
            sUnitDisplay.Set(o);
            unitDisplayDictionary.Add(o, sUnitDisplay);
            cooldowns.Add(o, new Timer(bUnit.placeCD));
            cooldowns[o].Reset();
        }
        SelectUnitDisplay(0);
    }

    private void UpdateCooldowns()
    {
        foreach (KeyValuePair<GameObject, BaseUnit> kvp in baseUnitDictionary)
        {
            Timer t = cooldowns[kvp.Key];
            t.UpdateTime();
            unitDisplayDictionary[kvp.Key].SetCD(t.ElapsedTime, t.TotalTime);
        }
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0) return;
        UpdateCooldowns();
        selectedIndicator.enabled = CanPlaceUnit();

        for (var i = 0; i < _unitKeys.Length; ++i)
        {
            if (!Input.GetKey(_unitKeys[i]) && !Input.GetButton(_buttonNames[i])) continue;

            currentUnitIndex = i;
            SelectUnitDisplay(i);
            selectedIndicator.sprite = _unitsToSpawn[currentUnitIndex].GetComponent<SpriteRenderer>().sprite;
            break;
        }

        // AxisControlPlacementCursor();
        KeyboardControlPlacementCursor();
    }

    private void SelectUnitDisplay(int i)
    {
        for (int j = 0; j < unitDisplays.Length; j++)
        {
            if (j != i)
            {
                unitDisplays[j].selected = false;
            } else
            {
                unitDisplays[i].selected = true;
            }
        }
    }

    private void KeyboardControlPlacementCursor()
    {
        // Place Units
        if (!Input.GetKey(_placeUnit) && !Input.GetButton(_placeButtonName))
        {
            placedUnit = false;
        }

        // Place unit down.
        if ((Input.GetKey(_placeUnit) || Input.GetButton(_placeButtonName)) && !placedUnit)
        {
            TryPlaceUnit();
        }

        if (!Input.GetKey(moveCursorRight) && !Input.GetKey(moveCursorLeft) && !Input.GetKey(moveCursorUp) && !Input.GetKey(moveCursorDown))
        {
            StopAllCoroutines();
        }

        // if (!_canMoveTimer.IsFinished()) return;

        if (Input.GetKeyDown(moveCursorLeft))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorLeft, -1, 0, _delayAfterKeyPressed, _delayWhileKeyHeld));
        }

        if (Input.GetKeyDown(moveCursorRight))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorRight, 1, 0, _delayAfterKeyPressed, _delayWhileKeyHeld));
        }

        if (Input.GetKeyDown(moveCursorDown))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorDown, 0, 1, _delayAfterKeyPressed, _delayWhileKeyHeld));
        }

        if (Input.GetKeyDown(moveCursorUp))
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(moveCursorUp, 0, -1, _delayAfterKeyPressed, _delayWhileKeyHeld));
        }
    }

    private void AxisControlPlacementCursor()
    {
        if (Input.GetAxisRaw(horizontalString) > -0.2f
            && Input.GetAxisRaw(horizontalString) < 0.2f
            && Input.GetAxisRaw(verticalString) > -0.2f
            && Input.GetAxisRaw(verticalString) < 0.2f)
        {
            StopAllCoroutines();
        }

        if (!Input.GetKey(_placeUnit) && !Input.GetButton(_placeButtonName))
        {
            placedUnit = false;
        }
        
        // Place unit down.
        if ((Input.GetKey(_placeUnit) || Input.GetButton(_placeButtonName)) && !placedUnit)
        {
            TryPlaceUnit();
        }

        /*
        if (!_canMoveTimer.IsFinished()) return;
        */

        if (Input.GetAxisRaw(horizontalString) < -0.2f)
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(horizontalString, -1, 0, 0.5f, 0.05f));
        }

        if (Input.GetAxisRaw(horizontalString) > 0.2f)
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(horizontalString, 1, 0, 0.5f, 0.05f));
        }

        if (Input.GetAxisRaw(verticalString) > 0.2f)
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(verticalString, 0, -1, 0.5f, 0.05f));
        }

        if (Input.GetAxisRaw(verticalString) < -0.2f)
        {
            StopAllCoroutines();
            StartCoroutine(StartMove(verticalString, 0, 1, 0.5f, 0.05f));
        }
    }

    protected abstract void TryPlaceUnit();

    protected abstract bool CanPlaceUnit();

    private IEnumerator StartMove(string key, int moveRow, int moveCol,
        float waitAfterFirstMove,
        float waitPastFirstMove)
    {
        bool first = true;
        MoveTileCursor(moveRow, moveCol);
        while (Input.GetAxisRaw(key) > 0.2f || Input.GetAxisRaw(key) < -0.2f)
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
            MoveTileCursor(moveRow, moveCol);

            yield return null;
        }
    }

    private void MoveTileCursor(int moveRow, int moveCol)
    {
        tileCursor.Move(moveRow, moveCol);
    }

    private IEnumerator StartMove(KeyCode key, int moveRow, int moveCol,
    float waitAfterFirstMove,
    float waitPastFirstMove)
    {
        bool first = true;
        MoveTileCursor(moveRow, moveCol);
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
            MoveTileCursor(moveRow, moveCol);
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