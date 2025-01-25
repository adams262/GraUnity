using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    private int currentPoint;

    void Start()
    {
        transform.position = patrolPoints[0].position;
        currentPoint = 0;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void Update()
    {
        
        if (Vector2.Distance(transform.position, patrolPoints[currentPoint].position) < 0.1f)
        {
            
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            currentPoint++;
        }
      
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
    }

    public void Hurt()
    {
        Destroy(this.gameObject);
    }
}
