using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class InteractableFramework : MonoBehaviour, IPointerClickHandler
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
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        
        // sets a trigger for cursor and player detection
        if (!GetComponent<BoxCollider>()) gameObject.AddComponent<BoxCollider>();
        _collider = GetComponent<BoxCollider>();
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = new Vector3(6, 6, 6);
    }
    
    private void Start()
    {
        _interactionManager = WordInteractionManager.Instance;
        _menuManager = MenuManager.Instance;
        
        _childInteractable = transform.GetChild(0).gameObject;
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
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (_playerDestinationSetter.target) return;
        if (_menuManager.activeMenuGroup) return;
        
        switch (pointerEventData.button)
        {
            case PointerEventData.InputButton.Left:
                // approach if far away
                if (!_isWithinPlayerRange)
                {
                    _playerDestinationSetter.target = gameObject.transform;
                    if (_newPopup) _newPopup.GetComponent<Popup>().Disappear(0.15f);
                    break;
                }
                
                // enter word interaction menu if close
                if (_newPopup) _newPopup.GetComponent<Popup>().Disappear(0.15f);
                _interactionManager.lastActiveFramework = this;
                _interactionManager.SetActiveInteraction(activeInteraction);
                activeInteraction.onLeftClick.Invoke();
                Debug.Log($"Left click on {gameObject.name}");
                break;
            
            case PointerEventData.InputButton.Right:
                // use if close
                if (!_isWithinPlayerRange) break;
                if (_newPopup) _newPopup.GetComponent<Popup>().Disappear(0.15f);
                activeInteraction.onRightClick?.Invoke();
                Debug.Log($"Right click on {gameObject.name}");
                break;
                
        }
    }

    public void OnMouseOver()
    {
        if (_playerDestinationSetter.target) return;
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
