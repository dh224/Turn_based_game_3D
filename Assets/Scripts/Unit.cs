using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.Timeline;

public class Unit : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public int movementAbility;
    public bool isOnTurn;

    IEnumerator moveCoroutine;
    private List<Node> pathWay;

    public void Move(List<Node> list)
    {
        pathWay = list;
        StartCoroutine(MoveWithPathway());
        this.movementAbility = (int) list[list.Count - 1].remainMovement;
        EventSystem.instance.EndMovement(list[list.Count - 1]);
    }
    IEnumerator MoveWithPathway()
    {
        foreach (var target in pathWay)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = MoveTo_Coroutine(target, 16f);
            StartCoroutine(moveCoroutine);
            yield return moveCoroutine;
        }
    }
    
    IEnumerator MoveTo_Coroutine(Node target, float speed)
    {
        while (transform.position.x != target.x || transform.position.z != target.y)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.y), speed * Time.deltaTime);
            yield return null;
        }
    }
}
