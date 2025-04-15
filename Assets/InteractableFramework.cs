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
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        
        gameObject.AddComponent<Rigidbody>();
    }
    
    private void Start()
    {
        _interactionManager = FindAnyObjectByType<WordInteractionManager>();
        _childInteractable = transform.GetChild(0).gameObject;
        _childInteractable = activeInteraction.prefab;
        
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void SetActiveInteraction(Interaction interaction)
    {
        activeInteraction = interaction;
        _childInteractable = activeInteraction.prefab;
    }

    public void OnLeftClick()
    {
        Debug.Log($"Left click on {activeInteraction.name}");
        _interactionManager.SetActiveWord(activeInteraction.id);
        activeInteraction.onLeftClick.Invoke();
    }
    
    public void OnRightClick()
    {
        Debug.Log($"Right click on {activeInteraction.id}");
        activeInteraction.onRightClick?.Invoke();
    }
}
