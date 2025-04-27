using UnityEngine;
using UnityEngine.EventSystems;

public class PositioningCube : MonoBehaviour, IPointerClickHandler
{
    private PlayerInventory _playerInventory;
    
    private void Start()
    {
        _playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _playerInventory.PlaceAt(transform);
        }
    }
}
