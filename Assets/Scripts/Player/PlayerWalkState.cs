using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.gameObject.name = "Player (Walking)";
    }

    public override void UpdateState(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseOver(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseExit(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleLeftClick(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleKeyboardInput(Player player)
    {
        throw new System.NotImplementedException();
    }
}
