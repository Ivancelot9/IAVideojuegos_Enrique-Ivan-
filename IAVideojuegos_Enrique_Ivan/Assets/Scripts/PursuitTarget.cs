using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitTarget : MonoBehaviour
{
    //En el tutorial de Unity "Pathfinding con NavMesh" https://www.youtube.com/watch?v=tFpv4xFZrq8
    //Se utilizo chat para la busqueda de informaci�n.
    public List<Transform> waypoints; // Lista para almacenar posiciones de waypoints
    public float arriveRadius = 1.0f; // Radio para considerar que se ha llegado a un waypoint
    public float force = 10.0f; // Fuerza de movimiento

    private Rigidbody rb; // Referencia al componente Rigidbody del objeto
    private int currentWaypointIndex = 0; // �ndice del waypoint actual
    private bool isMoving = false; // Estado de movimiento

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el componente Rigidbody al iniciar
    }

    void Update()
    {
        if (waypoints.Count > 0 && isMoving) // Verifica si hay waypoints y si el objeto est� en movimiento
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position; // Obtiene la posici�n del waypoint actual
            Vector3 direction = targetPosition - transform.position; // Calcula la direcci�n hacia el waypoint

            if (direction.magnitude < arriveRadius) // Verifica si se ha llegado al waypoint
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; // Avanza al siguiente waypoint en la lista
                if (currentWaypointIndex == 0 && waypoints.Count > 1) // Verifica si se ha completado un ciclo de waypoints
                {
                    // Opcional: Implementar una acci�n al completar el ciclo (por ejemplo, una animaci�n)
                }
                isMoving = false; // Detiene el movimiento
                rb.velocity = Vector3.zero; // Detiene el Rigidbody
            }
            else
            {
                Vector3 arriveVector = Arrive(targetPosition); // Calcula el vector de llegada
                rb.AddForce(arriveVector * force, ForceMode.Acceleration); // Aplica la fuerza al Rigidbody
            }
        }
    }

    // Funci�n para calcular el vector de llegada
    Vector3 Arrive(Vector3 target)
    {
        Vector3 desiredVelocity = target - transform.position; // Direcci�n deseada hacia el objetivo
        float distance = desiredVelocity.magnitude; // Distancia al objetivo
        float speed = rb.velocity.magnitude; // Velocidad actual del Rigidbody

        if (distance < arriveRadius) // Si la distancia es menor que el radio de llegada
        {
            speed = Mathf.Lerp(0, speed, distance / arriveRadius); // Reduce la velocidad gradualmente
        }

        desiredVelocity = desiredVelocity.normalized * speed; // Ajusta la velocidad deseada
        Vector3 steering = desiredVelocity - rb.velocity; // Calcula la fuerza de steering
        return steering; // Retorna la fuerza de steering
    }

    // Funci�n para agregar un waypoint
    public void AddWaypoint(Transform waypoint)
    {
        waypoints.Add(waypoint); // Agrega el waypoint a la lista
    }

    // Funci�n para eliminar un waypoint
    public void RemoveWaypoint(Transform waypoint)
    {
        waypoints.Remove(waypoint); // Elimina el waypoint de la lista
    }

    // Funci�n para iniciar el movimiento a lo largo de los waypoints
    public void StartMoving()
    {
        if (waypoints.Count > 0) // Verifica si hay waypoints en la lista
        {
            isMoving = true; // Establece el estado de movimiento en verdadero
        }
    }

    // M�todo para destruir el infiltrador
    public void DestroyInfiltrator()
    {
        gameObject.SetActive(false); // Desactiva el objeto
    }

    // M�todo para reaparecer el infiltrador en una posici�n dada
    public void ReappearInfiltrator(Vector3 position)
    {
        transform.position = position; // Establece la nueva posici�n
        gameObject.SetActive(true); // Reactiva el objeto
    }
}
