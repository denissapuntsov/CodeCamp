using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractableFramework : MonoBehaviour
{
    [SerializeField] private Interaction activeInteraction;
    
    private GameObject _childInteractable;
    private WordInteractionManager _interactionManager;
    private BoxCollider _collider;

    private void Reset()
    {
        if (GetComponent<Rigidbody>() == null) gameObject.AddComponent<Rigidbody>();
        
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        if (GetComponent<BoxCollider>() != null) return;
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(10, 10, 10);
    }
    
    private void Start()
    {
        _interactionManager = FindAnyObjectByType<WordInteractionManager>();
        _childInteractable = transform.GetChild(0).gameObject;
        
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
        _interactionManager.activeFramework = this;
        _interactionManager.SetActiveInteraction(activeInteraction);
        activeInteraction.onLeftClick.Invoke();
    }
    
    public void OnRightClick()
    {
        activeInteraction.onRightClick?.Invoke();
    }
}
