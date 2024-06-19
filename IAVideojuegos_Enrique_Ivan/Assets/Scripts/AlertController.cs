using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    public float AlertDuration = 5.0f;  // Duraci�n del estado de alerta en segundos
    private float alertTimer = 0.0f;  // Temporizador para el estado de alerta
    private Vector3 lastSeenPosition;  // �ltima posici�n vista del infiltrador

    public Rigidbody EnemyRigidbody;  // Referencia al componente Rigidbody del enemigo
    public AgentSenses Senses;  // Referencia al componente AgentSenses

    void Start()
    {
        // Busca el GameObject con la etiqueta "Infiltrator" y obtiene su Rigidbody
        GameObject infiltrator = GameObject.FindWithTag("Infiltrator");
        if (infiltrator != null)
        {
            EnemyRigidbody = infiltrator.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Infiltrator not found. Make sure you have an object tagged 'Infiltrator' in the scene.");
        }
    }

    void Update()
    {
        // Si no se encuentra el Rigidbody del enemigo, no hace nada
        if (EnemyRigidbody == null)
            return;

        // Comprueba si el infiltrador est� dentro del cono de visi�n
        bool targetInVision = Senses.TargetIsInVisionCone(EnemyRigidbody.position);
        if (targetInVision)
        {
            // Actualiza la �ltima posici�n vista del infiltrador
            lastSeenPosition = EnemyRigidbody.position;
            // Resetea el temporizador de alerta
            ResetAlertTimer();
        }

        // Incrementa el temporizador de alerta
        alertTimer += Time.deltaTime;
        if (alertTimer >= AlertDuration)
        {
            // L�gica para manejar la transici�n de vuelta al estado normal
        }
    }

    // Resetea el temporizador de alerta
    public void ResetAlertTimer()
    {
        alertTimer = 0.0f;
    }

    // Devuelve la �ltima posici�n vista del infiltrador
    public Vector3 GetLastSeenPosition()
    {
        return lastSeenPosition;
    }
}
