using System;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerClickHandler
{
    [Header("Level Position")] 
    public Tile activeTile;

    [Header("Item Slots")]
    public GameObject headgear;
    
    [Header("Transforms")]
    [SerializeField] private Transform headGearParent;

    [System.NonSerialized] public Popup popup;
    [System.NonSerialized] public AIPath aiPath;
    [System.NonSerialized] public float distanceThreshold;
    
    private Collider[] _hitColliders = new Collider[18];
    private List<Tile> _hitTiles;
    
    // FSM
    private PlayerBaseState _currentState;
    public PlayerBaseState CurrentState
    {
        get => _currentState;

        set
        {
            _currentState = value;
            _currentState.EnterState(player: this);
        }
    }
    
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerWalkState WalkState = new PlayerWalkState();
    public PlayerPlaceState PlaceState = new PlayerPlaceState();

    private void Start()
    {
        popup = GetComponentInChildren<Popup>();
        _hitTiles = new List<Tile>();
        aiPath = GetComponent<AIPath>();

        CurrentState = IdleState;
        //CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.UpdateState(player:this);
    }

    public void PutOnHeadgear(GameObject item)
    {
        // turn off trigger for tiles and collider for pointers
        if (item.GetComponentsInChildren<Collider>() != null)
        {
            foreach (Collider c in item.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }
        
        item.transform.SetParent(headGearParent, false);
        item.GetComponent<Rigidbody>().isKinematic = true;
        ResetTransform(item);
        headgear = item;
        
        // reset player destination to current position
        aiPath.destination = transform.position;
    }

    public void RemoveHeadgear(Tile tileTarget)
    {
        if (!headgear) return;

        headgear.transform.SetParent(p: null);
        headgear.GetComponent<Rigidbody>().isKinematic = false;
        ResetTransform(headgear);
        
        if (headgear.GetComponentsInChildren<Collider>() != null)
        {
            foreach (Collider c in headgear.GetComponentsInChildren<Collider>()) c.enabled = true;
        }
        
        headgear.transform.position = tileTarget.transform.position;
        headgear.transform.SetParent(p: tileTarget.parent.transform);
        headgear = null;
        
        foreach (Tile tile in _hitTiles) tile.CurrentState = tile.WalkState;
        tileTarget.CurrentState = tileTarget.HoldState;
        CurrentState = IdleState;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("pressed left click on player");
            HandleLeftClick();
        }
    }

    private void OnMouseOver()
    {
        CurrentState.OnMouseOver(this);
    }

    private void OnMouseExit()
    {
        CurrentState.OnMouseExit(this);
    }

    private void HandleLeftClick()
    {
        CurrentState.HandleLeftClick(this);
    }

    public void CalculatePlacementTiles()
    {
        _hitTiles.Clear();

        //Collider[] hitColliders = new Collider[8];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, 2.6f, _hitColliders, LayerMask.GetMask("Tile"));

        for (int i = 0; i < collidersHit; i++)
        {
            Tile tile = _hitColliders[i].GetComponent<Tile>();
            if (!tile) continue;
            if (!tile.hasPlayer && !tile.currentInteractable) _hitTiles.Add(tile);
        }
        
        foreach (Tile tile in _hitTiles) tile.CurrentState = tile.PlaceState;
    }

    public void ClearPlacementTiles()
    {
        foreach (Tile tile in _hitTiles) tile.CurrentState = tile.WalkState;
        CurrentState = IdleState;
    }

    private void ResetTransform(GameObject obj)
    { 
        obj.transform.localPosition = Vector3.zero; 
        obj.transform.localRotation = Quaternion.identity;
    }
}
