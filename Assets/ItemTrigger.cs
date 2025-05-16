using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private bool isBlocking = true;
    [SerializeField] private InteractionData key;
    [SerializeField] private UnityEvent onKeyAccepted;
    
    public void CheckForItem(InteractionData dataToCheck)
    {
        if (dataToCheck != key) return;
        onKeyAccepted?.Invoke();
        if (!isBlocking) return;
        Tile tile = GetComponent<Tile>();
        tile.CurrentState = tile.WalkState;
    }

    public void AnimateCurrentInteractable(string animationName)
    {
        transform.parent.GetComponentInChildren<InteractableFramework>().GetComponentInChildren<Animator>().Play(animationName);
    }
}
