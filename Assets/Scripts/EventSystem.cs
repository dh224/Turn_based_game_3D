using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance;

    public event Action<Unit> onEndMovement;
    public event Action<List<Node>> onLightingPathCubes;
    public event Action<List<Node>> onShowPathArrow;

    public event Action onUndobuttonClicked;

    public event Action<int, int> onClickableTileClicked;
    
    private void Awake()
    {
        instance = this;
    }

    public void UndobuttonClicked()
    {
        if (onUndobuttonClicked != null)
        {
            onUndobuttonClicked();
        }
    }

    public void ClickableTileClicked(int x, int y)
    {
        if (onClickableTileClicked != null)
        {
            onClickableTileClicked(x, y);
        }
    }

    public void EndMovement(Unit u)
    {
        if (onEndMovement != null)
        {
            onEndMovement(u);
        }
    }
    
    public void LightingPathCubes(List<Node> list)
    {
        if (onLightingPathCubes != null)
        {
            onLightingPathCubes(list);
        }
    }

    public void ShowPathArrow(List<Node> list)
    {
        if (onShowPathArrow != null)
        {
            onShowPathArrow(list);
        }
    }
}
