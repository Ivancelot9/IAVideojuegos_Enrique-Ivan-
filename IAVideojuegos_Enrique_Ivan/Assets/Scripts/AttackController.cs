using System.Collections;  // Necesario para IEnumerator y WaitForSeconds
using System.Collections.Generic;  // No se utiliza en este script pero est� presente por defecto
using UnityEngine;  // Necesario para trabajar con elementos de Unity

public class AttackController : MonoBehaviour
{
    public float MaxSpeed = 20.0f;  // Velocidad m�xima del guardia en ataque
    public float Force = 10.0f;  // Fuerza aplicada al guardia para moverse
    public float AttackDuration = 5.0f;  // Duraci�n del ataque antes de volver a la posici�n inicial
    private float attackTimer = 0.0f;  // Temporizador del ataque

    public Rigidbody rb;  // Rigidbody del guardia
    public Rigidbody EnemyRigidbody;  // Rigidbody del enemigo/infiltrador

    void FixedUpdate()
    {
        // Calcula la fuerza de direcci�n hacia el infiltrador
        Vector3 steeringForce = Seek(EnemyRigidbody.position);
        steeringForce = Vector3.ClampMagnitude(steeringForce, Force);  // Limita la fuerza de direcci�n
        rb.AddForce(steeringForce, ForceMode.Acceleration);  // Aplica la fuerza al guardia

        attackTimer += Time.deltaTime;  // Incrementa el temporizador del ataque
        if (InsideToleranceRadius(EnemyRigidbody.position))
        {
            // Si el infiltrador est� dentro del radio de tolerancia, inicia la destrucci�n del infiltrador
            StartCoroutine(DestroyInfiltratorAfterDelay(2.0f));
        }
        else if (attackTimer >= AttackDuration)
        {
            // Si el ataque dura m�s que AttackDuration, vuelve a la posici�n inicial
            ReturnToInitialPosition();
        }
    }

    // M�todo Seek para calcular la fuerza de direcci�n hacia la posici�n objetivo
    private Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredDirection = targetPosition - transform.position;  // Direcci�n deseada hacia el objetivo
        desiredDirection.Normalize();  // Normaliza la direcci�n
        return desiredDirection * MaxSpeed - rb.velocity;  // Calcula la fuerza de direcci�n
    }

    // Comprueba si el objetivo est� dentro del radio de tolerancia
    private bool InsideToleranceRadius(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < 1.0f;
    }

    // Coroutine para destruir al infiltrador despu�s de un retraso
    IEnumerator DestroyInfiltratorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // Espera por el tiempo especificado
        if (InsideToleranceRadius(EnemyRigidbody.position))
        {
            // Supone que el infiltrador tiene un m�todo para manejar su destrucci�n
            // infiltrator.DestroyInfiltrator();
        }
    }

    // M�todo para volver a la posici�n inicial (l�gica no implementada)
    private void ReturnToInitialPosition()
    {
        // Implementar la l�gica para volver a la posici�n inicial
    }
}
