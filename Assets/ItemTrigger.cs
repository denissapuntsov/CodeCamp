using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private InteractionData key;
    [SerializeField] private UnityEvent onKeyAccepted;
    
    public void CheckForItem(InteractionData dataToCheck)
    {
        if (dataToCheck != key) return;
        Tile tile = GetComponent<Tile>();
        tile.CurrentState = tile.WalkState;
        onKeyAccepted?.Invoke();
    }
}
