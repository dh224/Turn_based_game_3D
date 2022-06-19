using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public TileType[] tileTypes;
    private int[,] tiles;
    private Node[,] graph;
    private GameObject[,] mapInstances = new GameObject[mapSizeX, mapSizeY];
    public GameObject selectedUnitObject;
    public Unit selectedUnit;

    public Unit[] units;
    public static int mapSizeX = 10;
    public static int mapSizeY = 10;

    private UnitsMovements _unitsMovements;
    void Start()
    {
        _unitsMovements = new UnitsMovements();
        onloadEvents();
        generateMapData();
        generatePathfindingGraph();
        generateMapvisual();
        showMovementRange((int) selectedUnitObject.transform.position.x, (int) selectedUnitObject.transform.position.z);
        this.selectedUnit = selectedUnitObject.GetComponent<Unit>();
    }
    public void selectUnitMove(List<Node> route, float mobility)
    {
        MapUI.instance.clearMovementUIs();
        MapUI.instance.clearPathUIs();
        Command unitMoveTo = new MoveToTileCommand(selectedUnit, route, mobility);
        _unitsMovements.addCommand(unitMoveTo);
    }

    private void onloadEvents()
    {
        EventSystem.instance.onUndobuttonClicked += UndoCommand;
        EventSystem.instance.onEndMovement += unitEndMovement;
        EventSystem.instance.onLightingPathCubes += lightingCubes;
        EventSystem.instance.onClickableTileClicked += clickableTileClicked;
    }

    private void UndoCommand()
    {
        _unitsMovements.undoCommand();
    }

    private void clickableTileClicked(int x, int y)
    {
        if (selectedUnitObject == null)
        {
            //显示该砖块的信息
        }
        else
        {
            if (isInMovementRange(x, y))
            {
                float mobility = 0f;
                var route = generatePathWithSelectedUnit(x, y, out mobility);
                // Debug.Log("aaa " + mobility);
                selectUnitMove(route, mobility);
            }
        }
    }
    
    private void unitEndMovement(Unit end)
    {
        end.tileX = (int) end.transform.position.x;
        end.tileY = (int) end.transform.position.z;
        showMovementRange(end.tileX, end.tileY);
    }
    private void lightingCubes(List<Node> list)
    {
        foreach (var n in list)
        {
            mapInstances[n.x, n.y].GetComponent<Renderer>().material.color = Color.red;
        }
    }
    void generatePathfindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node(x, y);
                graph[x, y].nodeType = tileTypes[tiles[x, y]];
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (x != 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                }
                if(x < mapSizeX - 1)
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                if(y != 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if(y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
            }
        }
    }

    public bool isInMovementRange(int x1, int y1)
    {
        return getMovementRangeToCheck((int) selectedUnitObject.transform.position.x, (int) selectedUnitObject.transform.position.z)
            .Contains(graph[x1, y1]);
    }
    public List<Node> getMovementRangeToCheck(int x1, int y1)
    {
        List<Node> moveRange = new List<Node>();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y].isVisited_movementRange = false;
            }
        }
        Node root = graph[x1, y1];
        root.isVisited_movementRange = true;
        root.remainMovement =
            selectedUnit.movementAbility;
        Queue<Node> Q = new Queue<Node>();
        Q.Enqueue(root);
        while (Q.Count > 0)
        {
            Node v = Q.Dequeue();
            foreach (var vn in v.neighbours)
            {
                if (vn.isVisited_movementRange)
                    continue;
                if (v.remainMovement - (1 + vn.nodeType.extraCost) >= 0)
                {
                    vn.remainMovement = v.remainMovement - (1 + vn.nodeType.extraCost);
                    vn.isVisited_movementRange = true;
                    moveRange.Add(vn);
                    Q.Enqueue(vn);
                }
            }
        }
        return moveRange;
    }
    public List<Node> getMovementRange(int x1, int y1)
    {
        List<Node> moveRange = new List<Node>();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y].isVisited_movementRange = false;
            }
        }
        Node root = graph[x1, y1];
        root.isVisited_movementRange = true;
        root.remainMovement =
            selectedUnit.movementAbility;
        Debug.Log("起点的移动力：" + root.remainMovement );
        Queue<Node> Q = new Queue<Node>();
        Q.Enqueue(root);
        while (Q.Count > 0)
        {
            Node v = Q.Dequeue();
            foreach (var vn in v.neighbours)
            {
                if (vn.isVisited_movementRange)
                    continue;
                if (v.remainMovement - (1 + vn.nodeType.extraCost) >= 0)
                {
                    vn.remainMovement = v.remainMovement - (1 + vn.nodeType.extraCost);
                    vn.isVisited_movementRange = true;
                    moveRange.Add(vn);
                    Q.Enqueue(vn);
                }
            }
        }
        return moveRange;
    }
    void showMovementRange(int x1, int y1)
    {
        List<Node> moveRange = getMovementRange(x1, y1);
        MapUI.instance.showMovementRange(moveRange);
    }
    void generateMapData()
    {
        tiles = new int[mapSizeX, mapSizeY];
        tiles[0,0] = 0;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }
    }
    void generateMapvisual()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                GameObject go = Instantiate(tt.tileVisualPrefdab, new Vector3(x, 0,y), Quaternion.identity);
                Click_Tile ct = go.GetComponent<Click_Tile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
                mapInstances[x, y] = go;
            }
        }
    }

    public List<Node> generatePathWithSelectedUnit(int x, int y, out float moveUnitMobility)
    {
        moveUnitMobility = 0f;
        var path =  generatePath((int)selectedUnitObject.transform.position.x, (int)selectedUnitObject.transform.position.z, x, y, selectedUnit, out moveUnitMobility);
        return path;
    }
    public List<Node> generatePathWithSelectedUnit(int x, int y)
    {
        float moveUnitMobility = 0f;
        var path =  generatePath((int)selectedUnitObject.transform.position.x, (int)selectedUnitObject.transform.position.z, x, y, selectedUnit, out moveUnitMobility);
        return path;
    }
    public List<Node> generatePath(int startX, int startY, int targetX, int targetY, Unit moveUnit, out float mobilityCost)
    {
        mobilityCost = 0f;
        if (startX == targetX && startY == targetY)
        {
            List<Node> a = new List<Node>();
            a.Add(graph[startX, startY]);
            return a;
        }
        HashSet<Node> open = new HashSet<Node>();
        float preCost = h_Manhattan(startX, startY, targetX, targetY);
        float w1 = 1.5f;
        float w2 = 0.5f;
        open.Add(graph[startX, startY]);
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, float> gScore = new Dictionary<Node, float>();
        Dictionary<Node, float> fScore = new Dictionary<Node, float>();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                gScore[graph[x, y]] = Single.MaxValue;
                fScore[graph[x, y]] = Single.MaxValue;
            }
        }
        gScore[graph[startX,startY]] = 0;
        fScore[graph[startX, startY]] = h_Manhattan(startX,startY, targetX, targetY);
        while (open.Count > 0)
        {
            Node current = null;
            foreach (var n in open)
            {
                if (current == null)
                {
                    current = n;
                }
                else
                {
                    if (fScore[current] > fScore[n])
                    {
                        current = n;
                    }
                }
            }
            if (current.x == targetX && current.y == targetY)
            {
                mobilityCost = 0f;
                return reconstruct_path(cameFrom,current, moveUnit,out mobilityCost);
            }
            open.Remove(current);
            foreach (var neighbour in current.neighbours)
            {
                float tentative_gScore = gScore[current] + current.DistanceTo(neighbour);
                if (tentative_gScore < gScore[neighbour])
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentative_gScore;
                    float w = w1;
                    if (preCost / h_Manhattan(neighbour.x, neighbour.y, targetX, targetY) > 2f)
                    {
                        w = w2;
                    }
                    fScore[neighbour] = tentative_gScore + w * h_Manhattan(neighbour.x, neighbour.y, targetX, targetY);
                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    private List<Node> reconstruct_path(Dictionary<Node, Node> cameFrom,Node current, Unit unit, out float mobility)
    {
        mobility = 0f;
        List<Node> path = new List<Node>();
        path.Add(current);
        while (cameFrom.ContainsKey(current))
        {
            var next = cameFrom[current];
            mobility += (1 + next.nodeType.extraCost);
            path.Add(next);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
    private float h_Euclidean(int x1, int y1, int x2, int y2)
    {
        return Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));
    }
    private float h_Manhattan(int x1, int y1, int x2, int y2)
    {
        return  (float)(Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2));
    }
}
