using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableFramework : MonoBehaviour
{
    public InteractionData activeInteractionData;
    
    private GameObject _childInteractable;
    [System.NonSerialized] public WordInteractionManager interactionManager;
    private MenuManager _menuManager;
    private BoxCollider _collider;
    private Popup _popup;
    [System.NonSerialized] public bool isWithinPlayerRange;     
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
        
        // RequiresComponent(typeof(DynamicObstacle))
        if (!GetComponent<DynamicObstacle>()) gameObject.AddComponent<DynamicObstacle>();
    }
    
    private void Start()
    {
        interactionManager = WordInteractionManager.Instance;
        _menuManager = MenuManager.instance;
        _popup = GetComponentInChildren<Popup>();
        _player = FindAnyObjectByType<Player>();
        _aiPath = _player.GetComponent<AIPath>();
        _playerDestinationSetter = _player.GetComponent<AIDestinationSetter>();
        
        _childInteractable = transform.GetChild(0).gameObject;
        
        ReplaceChildInteractable();
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
            _playerDestinationSetter.target = null;
        }
        
        if (isWithinPlayerRange) TutorialManager.Instance.SetActiveTutorial("wear");
    }
}
