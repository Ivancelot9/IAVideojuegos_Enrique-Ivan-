using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveAmongUs : MonoBehaviour
{

    // Variables para el Movimiento
    private float horizontalMove;
    private float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;
    public float playerSpeed;

    // Variables para la Cámara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;

    // Variable para la Animación
    public Animator animator;

    // Variable Gravedad
    public float gravity = 9.8f;
    public float fallVelocity;

    // Variable Salto
    public float jumpForce;

    // Variable Agacharse
    private bool isCrouching = false;
    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        fallVelocity = -gravity * Time.deltaTime; // Inicializa la caída para evitar saltos no deseados
    }

    void Update()
    {
        // Cambiar el estado de agachado al presionar Control
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isCrouching = !isCrouching;
            animator.SetBool("agachado", isCrouching);

        }

        // Obtén la entrada del jugador solo si no está agachado
        if (!isCrouching)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");

            // Procesa la entrada
            playerInput = new Vector3(horizontalMove, 0, verticalMove);
            playerInput = Vector3.ClampMagnitude(playerInput, 1);

            // Configura la animación de caminar
            animator.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);

            // Calcula la dirección de la cámara
            camDirection();

            // Mueve el jugador en función de la entrada y la cámara
            movePlayer = playerInput.x * camRight + playerInput.z * camForward;
            movePlayer *= playerSpeed;

            // Ajusta la dirección en la que mira el jugador
            if (playerInput.magnitude > 0) // Solo mira hacia la dirección de movimiento si el jugador se está moviendo
            {
                player.transform.LookAt(player.transform.position + movePlayer);
            }

            // Configura la gravedad y el salto del jugador
            SetGravity();
            SaltoPersonaje();

            // Aplica el movimiento al jugador
            player.Move(movePlayer * Time.deltaTime);
        }

    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    private void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
            animator.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }
        animator.SetBool("isGrounded", player.isGrounded);
    }

    private void SaltoPersonaje()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            animator.SetTrigger("PlayerJump");
        }
    }

    

}


//Referencias

//https://www.youtube.com/watch?v=bbXCqHn7AI0&list=PLOc9rHNEUHlyryuY0PvipHTXyL2mBij9-&index=12