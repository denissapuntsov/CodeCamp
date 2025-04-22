using Pathfinding;
using UnityEngine;

public class InteractableFramework : MonoBehaviour
{
    [SerializeField] private Interaction activeInteraction;
    
    private GameObject _childInteractable;
    private WordInteractionManager _interactionManager;
    private MenuManager _menuManager;
    private BoxCollider _collider;
    private GameObject _newPopup;
    private bool _isWithinPlayerRange;     
    private GameObject _selectedPopupPrefab;
    private AIDestinationSetter _playerDestinationSetter;
    
    private void Reset()
    {
        // set layer to open interaction menus on click
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        
        // add physics if no Rigidbody present
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        
        // sets a trigger for cursor detection
        if (GetComponent<BoxCollider>())
        {
            _collider = GetComponent<BoxCollider>();
            return;
        }
        
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = new Vector3(3, 3, 3);
    }
    
    private void Start()
    {
        _interactionManager = WordInteractionManager.Instance;
        _childInteractable = transform.GetChild(0).gameObject;
        _menuManager = FindAnyObjectByType<MenuManager>();
        _selectedPopupPrefab = _interactionManager.approachPopupPrefab;
        _playerDestinationSetter = GameObject.FindWithTag("Player Parent").GetComponent<AIDestinationSetter>();
        
        ReplaceChildInteractable();
    }

    private void Update()
    {
        UpdatePopup();
    }

    private void ReplaceChildInteractable()
    {
        Destroy(_childInteractable);
        _childInteractable = Instantiate(activeInteraction.prefab, transform, false);
    }

    private void UpdatePopup()
    {
        _selectedPopupPrefab = _isWithinPlayerRange ? _interactionManager.usePopupPrefab : _interactionManager.approachPopupPrefab;
    }

    public void ReplaceInteraction(Interaction interaction)
    {
        activeInteraction = interaction;
        ReplaceChildInteractable();
    }

    public void OnMouseDown()
    {
        if (_playerDestinationSetter.target) return;
        if (_menuManager.activeMenuGroup) return;

        if (_newPopup) _newPopup.GetComponent<Popup>().Disappear(0.15f);
        
        if (!_isWithinPlayerRange && Input.GetMouseButtonDown(0))
        {
            _playerDestinationSetter.target = gameObject.transform;
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _interactionManager.lastActiveFramework = this;
            _interactionManager.SetActiveInteraction(activeInteraction);
            activeInteraction.onLeftClick.Invoke();
        }

        if (Input.GetMouseButtonDown(2))
        {
            activeInteraction.onRightClick?.Invoke();
        }
    }

    public void OnMouseOver()
    {
        if (_menuManager.activeMenuGroup || _newPopup) return;
        
        _newPopup = Instantiate(_selectedPopupPrefab, transform.position, Camera.main.transform.rotation, transform);
        if (!_isWithinPlayerRange) return;
        _newPopup.GetComponent<Popup>().SetUseText(activeInteraction.leftMouseText, activeInteraction.rightMouseText);
    }

    public void OnMouseExit()
    {
        if (_newPopup)
        {
            _newPopup.GetComponent<Popup>().Disappear();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isWithinPlayerRange = true;
        _playerDestinationSetter.target = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isWithinPlayerRange = false;
    }
}
