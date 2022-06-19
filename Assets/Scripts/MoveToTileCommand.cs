using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTileCommand : Command
{
    private Unit _unit;
    private List<Node> _route;
    private int movementAbility;
    private int remainMovementAbility;
    private float mobilityCost;
    public MoveToTileCommand(Unit moveUnit, List<Node> route, float mobilityCost)
    {
        _unit = moveUnit;
        _route = route;
        this.mobilityCost = mobilityCost;
    }
    public void execute()
    {
        _unit.Move(_route);
        _unit.minusMovementAbility(mobilityCost);
        Debug.Log("更新角色的移动力为：" + _unit.movementAbility);
    }

    public void undo()
    {
        _route.Reverse();
        _unit.Move(_route);
        _unit.plusMovementAbility(mobilityCost);
        Debug.Log("撤销后更新角色的移动力为：" + _unit.movementAbility);
    }
}
