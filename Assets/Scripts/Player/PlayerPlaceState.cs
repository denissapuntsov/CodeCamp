using UnityEngine;

public class PlayerPlaceState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.name = $"Player (Placing {player.headgear.name})";
    }

    public override void UpdateState(Player player)
    {
        HandleKeyboardInput(player);
    }

    public override void OnMouseOver(Player player)
    {
        player.ShowPopup("Cancel");
    }

    public override void OnMouseExit(Player player)
    {
        player.popup.Disappear();
    }

    public override void HandleLeftClick(Player player)
    {
        Debug.Log("Clicked on player in handle state");
        player.ClearPlacementTiles();
    }

    public override void HandleKeyboardInput(Player player)
    {

    }

    public override void ExitState(Player player)
    {
        
    }
}
