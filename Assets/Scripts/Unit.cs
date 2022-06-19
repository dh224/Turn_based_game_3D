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
        // Debug.Log("当前剩余的行动力：" + movementAbility);
    }

    public int minusMovementAbility(float m)
    {
        this.movementAbility -= (int)m;
        return this.movementAbility;
    }
    public int plusMovementAbility(float m)
    {
        this.movementAbility += (int)m;
        return this.movementAbility;
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
        EventSystem.instance.EndMovement(this);
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
