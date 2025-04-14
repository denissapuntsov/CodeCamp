using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string interactableName;
    private GameObject _childInteractable;
    private WordInteractionManager _interactionManager;

    [SerializeField] private UnityEvent onRightClick;

    private void Start()
    {
        _interactionManager = FindAnyObjectByType<WordInteractionManager>();
        _childInteractable = transform.GetChild(0).gameObject;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void OnLeftClick()
    {
        Debug.Log($"Left click on {interactableName}");
        _interactionManager.SetActiveWord(interactableName);
    }
    
    public void OnRightClick()
    {
        Debug.Log($"Right click on {interactableName}");
        onRightClick?.Invoke();
    }
}
