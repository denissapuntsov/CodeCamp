using UnityEngine;
using UnityEngine.EventSystems;

public class PositioningCube : MonoBehaviour//, IPointerClickHandler
{
    private Player _player;
    
    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _playerInventory.RemoveHeadgear(transform);
        }
    }*/
}
