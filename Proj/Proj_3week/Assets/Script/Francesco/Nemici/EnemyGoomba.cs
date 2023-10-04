using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoomba : Enemy
{
    [SerializeField] Transform[] patrolPoints;
    int currentPatrolPoint = 0;
    float movementSpeed = 3.0f; // Velocità di movimento del nemico



    private void Start()
    {
        // Imposta la posizione iniziale del nemico al primo punto di pattuglia
        transform.position = patrolPoints[currentPatrolPoint].position;
    }

    private void Update()
    {
        // Controlla se il nemico è vicino al punto di destinazione
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.1f)
        {
            // Passa al prossimo punto di pattuglia
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }

        MoveToNextPatrolPoint();
    }

    private void MoveToNextPatrolPoint()
    {
        Vector3 direction = (patrolPoints[currentPatrolPoint].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }
}
