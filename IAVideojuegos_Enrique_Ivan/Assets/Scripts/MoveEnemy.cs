using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    private Animator animator;
    private bool isAlert = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Cambiar de estado al presionar Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isAlert = !isAlert;
            animator.SetBool("isAlert", isAlert);
            Debug.Log("si jala la q");
        }

        // Lógica de movimiento de patrullaje
        if (!isAlert)
        {
            Patrol();
        }
        else
        {
            // Podrías agregar aquí la lógica específica para el estado de alerta
        }
    }

    void Patrol()
    {
        // Aquí añades el código de movimiento del enemigo cuando está patrullando
        // Por ejemplo, un simple movimiento hacia adelante:
      
    }
}
