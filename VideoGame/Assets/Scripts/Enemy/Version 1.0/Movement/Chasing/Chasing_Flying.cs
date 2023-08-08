using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing_Flying : MonoBehaviour // Testing chasing
{

    private const float speed = 40f;

    private Pathfinding path;

    private int currentPathIndex;
    private List<Vector2> PathVectorList;

    private Transform target;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        path = new Pathfinding(100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetPosition((Vector2) target.position);
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (PathVectorList != null)
        {
            Vector2 targetPosition = PathVectorList[currentPathIndex];
            if (Vector2.Distance((Vector2) transform.position, targetPosition) > 1f)
            {
                Vector2 moveDir = (targetPosition - (Vector2) transform.position).normalized;

                float distanceBefore = Vector2.Distance((Vector2)transform.position, targetPosition);
                transform.position = (Vector2) transform.position + moveDir * speed * Time.deltaTime;
            } else
            {
                currentPathIndex++;
                if (currentPathIndex >= PathVectorList.Count)
                {
                    StopMoving();
                }
            }
        } 
    }

    private void StopMoving()
    {
        PathVectorList = null;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        currentPathIndex = 0;
        PathVectorList = path.findPath(GetPosition(), targetPosition);

        if (PathVectorList != null && PathVectorList.Count > 1) {
            PathVectorList.RemoveAt(0);

        }
    }
}
