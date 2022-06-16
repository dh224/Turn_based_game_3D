using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTileCommand : Command
{
    private Unit _unit;
    private List<Node> _route;
    public MoveToTileCommand(Unit moveUnit, List<Node> route)
    {
        _unit = moveUnit;
        _route = route;
    }
    public void execute()
    {
        _unit.Move(_route);
    }
}
