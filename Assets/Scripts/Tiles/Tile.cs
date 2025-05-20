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
    private CapsuleCollider _capsule;
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
        _capsule = GetComponent<CapsuleCollider>();
        
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
        
        if (hasPlayer) { player.activeTile = this;}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        
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
        _capsule.enabled = colliderMode == "Item";
    }

    public void SetNewPath()
    {
        Path p = player.seeker.StartPath(player.transform.position, transform.position, p =>
        {
            player.aiPath.SetPath(p);
            player.CurrentState = player.WalkState;
        });
    }

    public void HideCurrentInteractable()
    {
        transform.parent.gameObject.GetComponentInChildren<InteractableFramework>().gameObject.SetActive(false);
    }
}
