using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public GameObject parent, selection, cross;
    
    public bool hasPlayer;
    public Player player;
    public InteractableFramework currentInteractable;

    private BoxCollider _collider;
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
        _collider = GetComponent<BoxCollider>();
        
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
        if (hasPlayer) { player.activeTile = this;}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_menuManager.activeMenuGroup) return;
        if (aiDestinationSetter.target) return;
        
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
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

    public void SetColliderSize(string colliderMode)
    {
        switch (colliderMode)
        {
            case "Item":
                _collider.center = new Vector3(0, 5, 0);
                _collider.size = new Vector3(1f, 8, 1f);
                break;
            case "Empty":
                _collider.center = new Vector3(0, 2, 0);
                _collider.size = new Vector3(1.3f, 2, 1.3f);
                break;
        }
    }

    public void SetNewPath()
    {
        Path p = player.seeker.StartPath(player.transform.position, transform.position, p =>
        {
            player.aiPath.SetPath(p);
            player.CurrentState = player.WalkState;
        });
    }
}
