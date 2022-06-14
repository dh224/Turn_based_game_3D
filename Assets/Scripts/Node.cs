using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public List<Node> neighbours;
    public int x;
    public int y;
    public TileType nodeType;
    public bool isVisited_movementRange;
    public float remainMovement;
    public float DistanceTo(Node n)
    {
        return Vector2.Distance(new Vector2(this.x, this.y), new Vector2(n.x, n.y));
    }
    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.neighbours = new List<Node>();
    }
}
