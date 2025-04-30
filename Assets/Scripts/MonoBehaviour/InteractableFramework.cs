using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableFramework : MonoBehaviour//, IPointerClickHandler
{
    public InteractionData activeInteractionData;
    
    private GameObject _childInteractable;
    private WordInteractionManager _interactionManager;
    private MenuManager _menuManager;
    private BoxCollider _collider;
    private Popup _popup;
    public bool isWithinPlayerRange;     
    private AIDestinationSetter _playerDestinationSetter;
    private Player _player;
    private AIPath _aiPath;
    
    private void Reset()
    {
        // set layer to open interaction menus on click
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        
        // add physics if no Rigidbody present
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        
        /* REDUNDANT: sets a trigger for cursor and player detection
        if (!GetComponent<BoxCollider>()) gameObject.AddComponent<BoxCollider>();
        _collider = GetComponent<BoxCollider>();
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = new Vector3(6, 6, 6);*/
        
        // RequiresComponent(typeof(DynamicObstacle))
        if (!GetComponent<DynamicObstacle>()) gameObject.AddComponent<DynamicObstacle>();
    }
    
    private void Start()
    {
        _interactionManager = WordInteractionManager.Instance;
        _menuManager = MenuManager.Instance;
        _popup = GetComponentInChildren<Popup>();
        _player = FindAnyObjectByType<Player>();
        _aiPath = _player.GetComponent<AIPath>();
        _playerDestinationSetter = _player.GetComponent<AIDestinationSetter>();
        
        _childInteractable = transform.GetChild(0).gameObject;
        
        ReplaceChildInteractable();
        
        // As an example, use the bounding box from the attached collider
        /*Bounds bounds = GetComponent<Collider>().bounds;
        var guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);*/
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

    public void HandleLeftClick()
    {
        // approach if far away
        if (!isWithinPlayerRange)
        {
            //_playerDestinationSetter.target = gameObject.transform;
            _player.distanceThreshold = 5.1f;
            _player.aiPath.destination = transform.position;
            _player.CurrentState = _player.WalkState;
            
            _popup.Disappear(0.15f);
            return;
        }
                
        // enter word interaction menu if close
        _popup.Disappear(0.15f);
        _interactionManager.lastActiveFramework = this;
        _interactionManager.SetActiveInteraction(activeInteractionData);
    }
    
    public void DisplayPopup()
    {
        string popupMode = isWithinPlayerRange ? "Use" : "Approach";
        _popup.UpdateSelection(popupMode);
        
        _popup.Appear();
        if (!isWithinPlayerRange) return;
        _popup.SetUseText(activeInteractionData.leftMouseText, activeInteractionData.rightMouseText);
    }

    public void RemovePopup()
    {
        _popup.Disappear();
    }

    private void Update()
    {
        isWithinPlayerRange = Vector2.Distance(
            new Vector2(_player.transform.position.x, 
                        _player.transform.position.z),
            new Vector2(transform.position.x, 
                        transform.position.z)
            ) <= 5.2f;
        
        if (_aiPath.reachedEndOfPath)
        {
            Debug.Log("reached end of path");
            _playerDestinationSetter.target = null;
        }
    }
}
