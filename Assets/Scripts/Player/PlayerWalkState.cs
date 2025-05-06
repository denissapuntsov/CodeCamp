using Pathfinding;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{

    public override void EnterState(Player player)
    {
        player.gameObject.name = "Player (Walking)";
        player.animator.SetBool(player.IsWalking, true);
    }

    public override void UpdateState(Player player)
    {
        if (player.aiPath.reachedEndOfPath)
        {
            Debug.Log("reached destination");
            player.CurrentState = player.IdleState;
        }
    }

    public override void OnMouseOver(Player player)
    {
        return;
    }

    public override void OnMouseExit(Player player)
    {
        return;
    }

    public override void HandleLeftClick(Player player)
    {
        return;
    }

    public override void HandleKeyboardInput(Player player)
    {
        return;
    }

    public override void ExitState(Player player)
    {
        player.animator.SetBool(player.IsWalking, false);
    }
}
