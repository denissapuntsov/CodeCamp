using Unity.VisualScripting;
using UnityEngine;

public class TileHoldState : TileBaseState
{
    public override void EnterState(Tile tile)
    {
        tile.currentInteractable = tile.parent.GetComponentInChildren<InteractableFramework>();
        tile.parent.name = $"Tile ({tile.currentInteractable.name})";
        tile.SetColliderSize("Item");
    }
     
    public override void UpdateState(Tile tile)
    {
        return;
    }
     
    public override void OnMouseEnter(Tile tile)
    {
        tile.selection.SetActive(true);
        tile.currentInteractable.DisplayPopup();
    }

    public override void OnMouseExit(Tile tile)
    {
        tile.selection.SetActive(false);
        tile.currentInteractable.RemovePopup();
    }

    public override void HandleLeftClick(Tile tile)
    {
        tile.selection.SetActive(false);
        tile.currentInteractable.HandleLeftClick();
    }

    public override void HandleRightClick(Tile tile)
    {
        UseHeldInteractableByType(tile);
    }

    private void UseHeldInteractableByType(Tile tile)
    {
        tile.currentInteractable.RemovePopup();
        switch (tile.currentInteractable.activeInteractionData.interactionType)
        {
            case Type.Clothes:
                if (tile.player.headgear) return;
                tile.player.PutOnHeadgear(tile.currentInteractable.gameObject);
                tile.currentInteractable = null;
                tile.CurrentState = tile.WalkState;
                break;
        }
    }
}
