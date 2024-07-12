using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Navegation : MonoBehaviour
{

    // Variables para la navegaci�n
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent amongus;
    private Vector3 initialPosition;
    private bool isFollowing = false;
    private bool returning = false;
    private bool waiting = false;

    // Variables para el cono de visi�n
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private LayerMask targetMask; // M�scara de capa para detectar solo objetos espec�ficos
    [SerializeField] private float waitTime = 3f; // Tiempo de espera antes de seguir al objetivo

    private void Awake()
    {
        amongus = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isFollowing)
        {
            amongus.destination = movePositionTransform.position;

            if (Vector3.Distance(transform.position, movePositionTransform.position) < 1f)
            {
                isFollowing = false;
                returning = true;
            }
        }
        else if (returning)
        {
            amongus.destination = initialPosition;

            if (Vector3.Distance(transform.position, initialPosition) < 1f)
            {
                returning = false;
            }
        }
        else if (!waiting)
        {
            DetectTarget();
        }
    }

    private void DetectTarget()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (Collider target in targetsInView)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                // El objetivo est� a la vista, comenzar a seguir
                isFollowing = true;
                break; // Solo sigue al primer objetivo encontrado
            }
        }
    }

    // Se elimin� la corrutina WaitAndFollow ya que no es necesaria

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = transform.position;
        Vector3 forward = transform.forward * viewDistance;

        // Dibujar la l�nea hacia adelante
        Gizmos.DrawLine(startPoint, startPoint + forward);

        // Dibujar la l�nea l�mite izquierda
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Gizmos.DrawLine(startPoint, startPoint + leftBoundary);

        // Dibujar la l�nea l�mite derecha
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;
        Gizmos.DrawLine(startPoint, startPoint + rightBoundary);

        // Dibujar l�neas para mostrar el cono
        Gizmos.DrawLine(startPoint + leftBoundary, startPoint + rightBoundary);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == movePositionTransform && isFollowing)
        {
            isFollowing = false;
            returning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == movePositionTransform && isFollowing)
        {
            isFollowing = false;
            returning = true;
        }
    }
}

