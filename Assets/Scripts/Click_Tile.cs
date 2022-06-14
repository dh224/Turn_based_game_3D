using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Tile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;

    void Start()
    {
        
    }
    void OnMouseUp()
    {
        if (map.isInMovementRange(tileX, tileY) == true)
        {
            List<Node> path =  map.generatePathWithSelectedUnit(tileX, tileY);
            map.selectUnitMove(path);
        }
    }
    void OnMouseEnter()
    {
        if (map.isInMovementRange(tileX, tileY) == true)
        {
            List<Node> path =  map.generatePathWithSelectedUnit(tileX, tileY);
            MapUI.instance.showPathUI(path);
        }
    }
}
