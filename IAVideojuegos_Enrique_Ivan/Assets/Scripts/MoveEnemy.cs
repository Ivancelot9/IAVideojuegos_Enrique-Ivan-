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

        // L�gica de movimiento de patrullaje
        if (!isAlert)
        {
            Patrol();
        }
        else
        {
            // Podr�as agregar aqu� la l�gica espec�fica para el estado de alerta
        }
    }

    void Patrol()
    {
        // Aqu� a�ades el c�digo de movimiento del enemigo cuando est� patrullando
        // Por ejemplo, un simple movimiento hacia adelante:
      
    }
}
