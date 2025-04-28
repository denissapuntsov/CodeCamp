using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public GameObject parent;
    
    public bool hasPlayer;
    public PlayerInventory player;
    public InteractableFramework currentInteractable;

    private MeshCollider _collider;
    private Material _material;
    public AIDestinationSetter aiDestinationSetter;
    private MenuManager _menuManager;

    private TileBaseState _currentState;
    public TileWalkState WalkState = new TileWalkState();
    public TileHoldState HoldState = new TileHoldState();
    public TilePlaceState PlaceState = new TilePlaceState();

    private void Start()
    {
        _material = GetComponentInParent<MeshRenderer>().material;
        player = FindAnyObjectByType<PlayerInventory>();
        aiDestinationSetter = player.GetComponent<AIDestinationSetter>();
        _menuManager = FindAnyObjectByType<MenuManager>();
        _collider = GetComponent<MeshCollider>();
        
        currentInteractable = parent?.GetComponentInChildren<InteractableFramework>();
        parent.name = currentInteractable != null ? $"Tile ({currentInteractable.name})" : "Tile (Empty)";

        _currentState = currentInteractable != null ? HoldState : WalkState;
        Debug.Log($"{parent.name} in state {_currentState}");
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

    public void SwitchState(TileBaseState state)
    {
        _currentState = state;
        Debug.Log($"entered {_currentState}");
        state.EnterState(this);
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
