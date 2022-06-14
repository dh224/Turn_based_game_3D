using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public static MapUI instance;
    public GameObject movementRangeUI;
    public GameObject UIPath;
    public GameObject UIPathCor;
    public GameObject UIPathArrow;
    public float movementRangeHeight = 1f;
    public float path_pathArrowHeight = 1f;
    
    private List<GameObject> movementUIs ;
    private List<GameObject> pathUIs;
    void Awake()
    {
        movementUIs = new List<GameObject>();
        pathUIs = new List<GameObject>();
        instance = this;
    }

    public void clearPathUIs()
    {
        foreach (var uis in pathUIs)
        {
            Destroy(uis);
        }
        pathUIs.Clear();
    }
    public void clearMovementUIs()
    {
        foreach (var ui in movementUIs)
        {
            Destroy(ui);
        }
        movementUIs.Clear();
    }
    public void showMovementRange(List<Node> list)
    {
        clearMovementUIs();
        foreach (var node in list)
        {
            GameObject go = Instantiate(movementRangeUI, new Vector3(node.x, movementRangeHeight,node.y), Quaternion.identity);
            movementUIs.Add(go);
        }
    }

    public void showPathUI(List<Node> list)
    {
        clearPathUIs();
        for (int i = 1; i < list.Count; i++)
        {
            if (i < list.Count - 1)
            {
                Node last = list[i - 1];
                Node cur = list[i];
                Node next = list[i + 1];
                Vector2 last_cor = new Vector2(last.x, last.y);
                Vector2 cur_cor = new Vector2(cur.x, cur.y);
                Vector2 next_cor = new Vector2(next.x, next.y);
                Vector2 orientation_last = (cur_cor - last_cor).normalized;
                Vector2 orientation_next = (next_cor - cur_cor).normalized;
                if (orientation_last.x == orientation_next.x && orientation_last.y == orientation_next.y)
                {
                    if (orientation_last.x == 1 || orientation_last.x == -1)
                    {
                        GameObject go = Instantiate(UIPath, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                            Quaternion.Euler(0,0,0));
                        pathUIs.Add(go);
                    }else if (orientation_last.y == 1 || orientation_last.y == -1)
                    {
                        GameObject go = Instantiate(UIPath, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                            Quaternion.Euler(0,90,0));
                        pathUIs.Add(go);
                    } 
                }
                else
                {
                    // 0 left 1 right 
                    int horizontalCode = 0;
                    // 0 top 1 bottom
                    int verticalCode = 0;
                    if (orientation_last.x == -1 || orientation_next.x == 1)
                    {
                        horizontalCode = 1;
                    }
                    if (orientation_last.y == 1 || orientation_next.y == -1)
                    {
                        verticalCode = 1;
                    }
                    if (horizontalCode == 0 && verticalCode == 0)
                    {
                        GameObject go = Instantiate(UIPathCor, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                            Quaternion.Euler(0,180,0));
                        pathUIs.Add(go);
                    }else if (horizontalCode == 1 && verticalCode == 0)
                    {
                        GameObject go = Instantiate(UIPathCor, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                            Quaternion.Euler(0,-90,0));
                        pathUIs.Add(go);
                    }else if (horizontalCode == 0 && verticalCode == 1)
                    {
                        
                        GameObject go = Instantiate(UIPathCor, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                            Quaternion.Euler(0,90,0));
                        pathUIs.Add(go);
                    }else if (horizontalCode == 1 && verticalCode == 1)
                    {
                        GameObject go = Instantiate(UIPathCor, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                            Quaternion.Euler(0,0,0));
                        pathUIs.Add(go);
                    }
                }

            }
            else
            {
                Node cur = list[i];
                Node last = list[i - 1];
                Vector2 cur_cor = new Vector2(cur.x, cur.y);
                Vector2 last_cor = new Vector2(last.x, last.y);
                Vector2 orientation = (cur_cor - last_cor).normalized;
                if (orientation.x == 1)
                {
                    GameObject go = Instantiate(UIPathArrow, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                        Quaternion.Euler(0,180,0));
                    pathUIs.Add(go);
                }else if (orientation.y == -1)
                {
                    GameObject go = Instantiate(UIPathArrow, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                        Quaternion.Euler(0,270,0));
                    pathUIs.Add(go);
                }else if (orientation.x == -1)
                {
                    GameObject go = Instantiate(UIPathArrow, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                        Quaternion.Euler(0,0,0));
                    pathUIs.Add(go);
                }else if (orientation.y == 1)
                {
                    GameObject go = Instantiate(UIPathArrow, new Vector3(cur.x, path_pathArrowHeight, cur.y),
                        Quaternion.Euler(0,90,0));
                    pathUIs.Add(go);
                }
            }
            
        }
    }
}
