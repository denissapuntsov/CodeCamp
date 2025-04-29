using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public GameObject parent;
    
    public bool hasPlayer;
    public Player player;
    public InteractableFramework currentInteractable;

    private MeshCollider _collider;
    public AIDestinationSetter aiDestinationSetter;
    private MenuManager _menuManager;
    
    // FSM
    private TileBaseState _currentState;
    public TileBaseState CurrentState
    {
        get => _currentState;

        set
        {
            _currentState = value;
            _currentState.EnterState(tile: this);
        }
    }
    
    public TileWalkState WalkState = new TileWalkState();
    public TileHoldState HoldState = new TileHoldState();
    public TilePlaceState PlaceState = new TilePlaceState();

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        aiDestinationSetter = player.GetComponent<AIDestinationSetter>();
        _menuManager = FindAnyObjectByType<MenuManager>();
        _collider = GetComponent<MeshCollider>();
        
        currentInteractable = parent.GetComponentInChildren<InteractableFramework>();

        _currentState = currentInteractable != null ? HoldState : WalkState;
        _currentState.EnterState(this);
    }

    private void Update()
    {
        SetCollision();
    }

    private void SetCollision()
    {
        hasPlayer = Vector2.Distance(
            new Vector2(player.transform.position.x, 
                player.transform.position.z),
            new Vector2(transform.position.x, 
                transform.position.z)
        ) <= 0.5f;

        _collider.enabled = !hasPlayer;
        player.activeTile = hasPlayer ? this : null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_menuManager.activeMenuGroup) return;
        if (aiDestinationSetter.target) return;
        
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (aiDestinationSetter.target) return;
                _currentState.HandleLeftClick(this);
                break;
            
            case PointerEventData.InputButton.Right:
                _currentState.HandleRightClick(this);
                break;
        }
    }

    private void OnMouseEnter()
    {
        if (_menuManager.activeMenuGroup) return;
        if (aiDestinationSetter.target) return;
        _currentState.OnMouseEnter(this);
    }

    private void OnMouseExit()
    {
        if (_menuManager.activeMenuGroup) return;
        if (aiDestinationSetter.target) return;
        _currentState.OnMouseExit(this);
    }
}
