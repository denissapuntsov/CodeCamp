using Unity.VisualScripting;
using UnityEngine;

public class TileHoldState : TileBaseState
{
    public override void EnterState(Tile tile)
    {
        return;
    }
     
    public override void UpdateState(Tile tile)
    {
        return;
    }
     
    public override void OnMouseEnter(Tile tile)
    {
        tile.currentInteractable.DisplayPopup();
    }

    public override void OnMouseExit(Tile tile)
    {
        tile.currentInteractable.RemovePopup();
    }

    public override void HandleLeftClick(Tile tile)
    {
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
                tile.player.PutOn(tile.currentInteractable.gameObject);
                tile.currentInteractable = null;
                tile.SwitchState(tile.WalkState);
                break;
        }
    }
}
