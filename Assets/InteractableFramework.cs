using UnityEngine;

public class InteractableFramework : MonoBehaviour
{
    [SerializeField] private Interaction activeInteraction;
    
    [Header("Scene References")]
    private GameObject _childInteractable;
    private WordInteractionManager _interactionManager;
    private MenuManager _menuManager;
    private BoxCollider _collider;

    private void Reset()
    {
        // set layer to open interaction menus on click
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        
        // add physics if no Rigidbody present
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        
        // sets a trigger for cursor detection
        if (GetComponent<BoxCollider>()) return;
        
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(10, 10, 10);
    }
    
    private void Start()
    {
        _interactionManager = FindAnyObjectByType<WordInteractionManager>();
        _childInteractable = transform.GetChild(0).gameObject;
        _menuManager = FindAnyObjectByType<MenuManager>();
        
        ReplaceChildInteractable();
    }

    private void ReplaceChildInteractable()
    {
        Destroy(_childInteractable);
        _childInteractable = Instantiate(activeInteraction.prefab, transform, false);
    }

    public void ReplaceInteraction(Interaction interaction)
    {
        activeInteraction = interaction;
        ReplaceChildInteractable();
    }
    
    public void OnLeftClick()
    {
        if (_menuManager.activeMenuGroup) return;
        
        _interactionManager.lastActiveFramework = this;
        _interactionManager.SetActiveInteraction(activeInteraction);
        activeInteraction.onLeftClick.Invoke();
    }
    
    public void OnRightClick()
    {
        activeInteraction.onRightClick?.Invoke();
    }
}
