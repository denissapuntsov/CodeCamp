using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableFramework : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InteractionData activeInteractionData;
    //public Tile currentTile;
    
    private GameObject _childInteractable;
    private WordInteractionManager _interactionManager;
    private MenuManager _menuManager;
    private BoxCollider _collider;
    private Popup _popup;
    private bool _isWithinPlayerRange;     
    private AIDestinationSetter _playerDestinationSetter;
    private PlayerInventory _player;
    private AIPath _aiPath;
    
    private void Reset()
    {
        // set layer to open interaction menus on click
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        
        // add physics if no Rigidbody present
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        
        // sets a trigger for cursor and player detection
        if (!GetComponent<BoxCollider>()) gameObject.AddComponent<BoxCollider>();
        _collider = GetComponent<BoxCollider>();
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = new Vector3(6, 6, 6);
        
        // RequiresComponent(typeof(DynamicObstacle))
        if (!GetComponent<DynamicObstacle>()) gameObject.AddComponent<DynamicObstacle>();
    }
    
    private void Start()
    {
        _interactionManager = WordInteractionManager.Instance;
        _menuManager = MenuManager.Instance;
        _popup = GetComponentInChildren<Popup>();
        _player = FindAnyObjectByType<PlayerInventory>();
        _aiPath = _player.GetComponent<AIPath>();
        _playerDestinationSetter = _player.GetComponent<AIDestinationSetter>();
        
        _childInteractable = transform.GetChild(0).gameObject;
        
        ReplaceChildInteractable();
        
        // As an example, use the bounding box from the attached collider
        Bounds bounds = GetComponent<Collider>().bounds;
        var guo = new GraphUpdateObject(bounds);

        // Set some settings
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }

    private void ReplaceChildInteractable()
    {
        Destroy(_childInteractable);
        _childInteractable = Instantiate(activeInteractionData.prefab, transform, false);
        gameObject.name = activeInteractionData.id;
    }

    public void ReplaceInteraction(InteractionData interactionData)
    {
        activeInteractionData = interactionData;
        ReplaceChildInteractable();
    }
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (_menuManager.activeMenuGroup) return;
        if (_playerDestinationSetter.target) return;
        
        switch (pointerEventData.button)
        {
            case PointerEventData.InputButton.Left:
                HandleLeftClick();
                break;
            
            case PointerEventData.InputButton.Right:
                HandleRightClick();
                break;
                
        }
    }

    private void HandleRightClick()
    {
        // use if close
        if (!_isWithinPlayerRange) return;
        if (_popup) _popup.GetComponent<Popup>().Disappear(0.15f);
        UseByType();
    }

    private void HandleLeftClick()
    {
        // approach if far away
        if (!_isWithinPlayerRange)
        {
            _playerDestinationSetter.target = gameObject.transform;
            _popup.Disappear(0.15f);
            return;
        }
                
        // enter word interaction menu if close
        _popup.Disappear(0.15f);
        _interactionManager.lastActiveFramework = this;
        _interactionManager.SetActiveInteraction(activeInteractionData);
    }

    private void UseByType()
    {
        switch (activeInteractionData.interactionType)
        {
            case Type.Clothes:
                _player.PutOn(gameObject);
                break;
        }
    }

    public void OnMouseOver()
    {
        if (_menuManager.activeMenuGroup) return;
        if (_playerDestinationSetter.target) return;
        
        // set corresponding popup interface
        string popupMode = _isWithinPlayerRange ? "Use" : "Approach";
        _popup.UpdateSelection(popupMode);
        
        _popup.Appear();
        if (!_isWithinPlayerRange) return;
        _popup.SetUseText(activeInteractionData.leftMouseText, activeInteractionData.rightMouseText);
    }

    public void OnMouseExit()
    {
        _popup.Disappear();
    }

    private void Update()
    {
        if (_aiPath.reachedEndOfPath)
        {
            _playerDestinationSetter.target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isWithinPlayerRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isWithinPlayerRange = false;
    }
}
