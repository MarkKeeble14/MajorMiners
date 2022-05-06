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
    [SerializeField] private int money;
    [SerializeField] private int startMoney;
    public int Money
    {
        get { return money; }
    }
    public bool placing;
    private GameObject _numberPopup;
    public void AlterMoney(int change)
    {
        money += change;
    }
    public void AlterMoney(int change, Vector3 numberPosition)
    {
        if (change == 0) return;
        money += change;
        GameObject spawned = Instantiate(_numberPopup, numberPosition, Quaternion.identity);
        GameObject numberPopup = spawned.transform.GetChild(0).gameObject;
        if (change > 0)
        {
            numberPopup.GetComponent<PopupText>().Set("$" + change.ToString(), Color.green);
        } else
        {
            numberPopup.GetComponent<PopupText>().Set("-$" + (-1 * change).ToString(), Color.red);
        }
    }

    [SerializeField] protected TileManager tileManager;
    [SerializeField] protected SpriteRenderer selectedIndicator;
    [SerializeField] protected TileCursor tileCursor;
    [SerializeField] protected SpriteRenderer tileCursorSpriteRenderer;
    [SerializeField] private GameObject buyableUnitDisplay;
    [SerializeField] private Transform buyableUnitDisplayParent;
    [SerializeField] private LeftOrRight tooltipLeanDirection;
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
    [SerializeField] private KeyCode openTooltip;
    
    [SerializeField] private float _delayAfterKeyPressed = 0.5f;
    [SerializeField] private float _delayWhileKeyHeld = 0.05f;

    protected Dictionary<GameObject, BaseUnit> baseUnitDictionary = new Dictionary<GameObject, BaseUnit>();
    protected Dictionary<GameObject, UnitDisplay> unitDisplayDictionary = new Dictionary<GameObject, UnitDisplay>();
    protected Dictionary<GameObject, Timer> cooldowns = new Dictionary<GameObject, Timer>();
    private UnitDisplay[] unitDisplays;

    protected int currentUnitIndex;
    protected bool placedUnit;
    [SerializeField] private bool tooltipOpen = false;

    [SerializeField] protected List<GameObject> spawnedUnits = new List<GameObject>();

    [SerializeField] private int moneyPerResourceTick;
    [SerializeField] private float secondsBetweenResourceTick;
    private Timer passiveIncomeTimer;
    private WorldTile centerTile;

    private void Awake()
    {
        _numberPopup = (GameObject)Resources.Load("PopupText/NumberPopupCanvas");
    }

    private void Start()
    {
        tileCursorSpriteRenderer = tileCursor.GetComponent<SpriteRenderer>();
    }

    public void ResetPlayer()
    {
        DestroyAllSpawns();
        money = startMoney;
        baseUnitDictionary.Clear();
        cooldowns.Clear();
        foreach (KeyValuePair<GameObject, UnitDisplay> kvp in unitDisplayDictionary)
        {
            Destroy(kvp.Value.gameObject);
        }
        unitDisplayDictionary.Clear();
        centerTile = null;
        SetPlayer();
    }

    public void DestroyAllSpawns()
    {
        while (spawnedUnits.Count > 0)
        {
            GameObject spawned = spawnedUnits[0];
            spawnedUnits.RemoveAt(0);
            Destroy(spawned);
        }
    }

    private void SetPlayer()
    {
        centerTile = tileManager.GetTile(tileManager.Rows / 2, tileManager.Columns / 2);
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
            sUnitDisplay.Set(o, i + 1);
            sUnitDisplay.SetLean(tooltipLeanDirection);
            unitDisplayDictionary.Add(o, sUnitDisplay);
            Timer t = new Timer(bUnit.placeCD);
            cooldowns.Add(o, t);
            t.Reset();
        }
        SelectUnitDisplay(0);
        if (secondsBetweenResourceTick > 0)
            passiveIncomeTimer = new Timer(secondsBetweenResourceTick);
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

    private void CloseAllTooltips()
    {
        for (int i = 0; i < unitDisplays.Length; i++)
        {
            unitDisplays[i].CloseTooltip();
        }
        tooltipOpen = false;
    }


    private void OpenTooltip(int index)
    {
        for (int i = 0; i < unitDisplays.Length; i++)
        {
            if (i == index)
            {
                if (unitDisplays[i].IsOpen)
                {
                    tooltipOpen = false;
                    unitDisplays[i].CloseTooltip();
                } else
                {
                    tooltipOpen = true;
                    unitDisplays[i].OpenTooltip();
                }
                return;
            } else
            {
                unitDisplays[i].CloseTooltip();
            }
        }
        tooltipOpen = false;
    }

    public virtual void UpdatePlayer()
    {
        if (Time.timeScale == 0) return;

        if (secondsBetweenResourceTick > 0)
        {
            if (passiveIncomeTimer.IsFinished())
            {
                AlterMoney(moneyPerResourceTick, centerTile.transform.position);
                passiveIncomeTimer.Reset();
            } else
            {
                passiveIncomeTimer.UpdateTime();
            }
        }

        // Update cooldowns
        UpdateCooldowns();

        // Only show placement indicator if the unit can be placed
        selectedIndicator.enabled = CanPlaceUnit();

        // Movement
        // AxisControlPlacementCursor();
        KeyboardControlPlacementCursor();

        for (var i = 0; i < _unitKeys.Length; ++i)
        {
            if (!Input.GetKey(_unitKeys[i]) && !Input.GetButton(_buttonNames[i])) continue;
            
            // Updating current unit index (player has selected a new unit)
            currentUnitIndex = i;
            SelectUnitDisplay(i);
            selectedIndicator.sprite = _unitsToSpawn[currentUnitIndex].GetComponent<SpriteRenderer>().sprite;
            if (tooltipOpen)
            {
                CloseAllTooltips();
                OpenTooltip(currentUnitIndex);
            }

            break;
        }

        // Deal with tooltips
        if (Input.GetKeyDown(openTooltip))
        {
            OpenTooltip(currentUnitIndex);
        }
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