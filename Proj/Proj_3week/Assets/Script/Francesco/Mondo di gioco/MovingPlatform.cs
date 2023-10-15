using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5.0f;
    [SerializeField] Transform[] pathPoints; // array di posizioni
    private int currentPathPoint = 0;

    private bool playerIsOnPlatform = false; // giocatore sulla piattaforma
    private Transform playerTransform; // Riferimento al trasform del giocatore


    private void Start()
    {
        // Imposta la posizione iniziale del nemico al primo punto di pattuglia
        transform.position = pathPoints[currentPathPoint].position;
    }

    private void Update()
    {
        // Controlla se il nemico è vicino alla destinazione
        if (Vector3.Distance(transform.position, pathPoints[currentPathPoint].position) < 0.1f)
        {
            // Passa alla prossima destinazione
            currentPathPoint = (currentPathPoint + 1) % pathPoints.Length;
        }

        MoveToNextPatrolPoint();

        // Se il giocatore è sulla piattaforma, imparenta e aggiorna la sua posizione
        if (playerIsOnPlatform && playerTransform != null)
        {
            playerTransform.parent = transform;
        }
    }

    private void MoveToNextPatrolPoint() // funzione per aggiornare il movimento della piattaforma alla prossima destinazione
    {
        Vector3 direction = (pathPoints[currentPathPoint].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();

        if (playerCheck == null)
        {
            return;
        }

        playerIsOnPlatform = true; // Il giocatore è salito sulla piattaforma
        playerTransform = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IPlayer playerCheck = collision.GetComponent<IPlayer>();

        if (playerCheck == null)
        {
            return;
        }

        playerIsOnPlatform = false; // Il giocatore è sceso dalla piattaforma
        playerTransform.parent = null; // Rimuove il parent
    }
}
