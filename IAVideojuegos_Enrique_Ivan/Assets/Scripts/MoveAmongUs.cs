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

    // Variables para la C�mara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;

    // Variable para la Animaci�n
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
        fallVelocity = -gravity * Time.deltaTime; // Inicializa la ca�da para evitar saltos no deseados
    }

    void Update()
    {
        // Cambiar el estado de agachado al presionar Control
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isCrouching = !isCrouching;
            animator.SetBool("agachado", isCrouching);

        }

        // Obt�n la entrada del jugador solo si no est� agachado
        if (!isCrouching)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");

            // Procesa la entrada
            playerInput = new Vector3(horizontalMove, 0, verticalMove);
            playerInput = Vector3.ClampMagnitude(playerInput, 1);

            // Configura la animaci�n de caminar
            animator.SetFloat("PlayerWalkVelocity", playerInput.magnitude * playerSpeed);

            // Calcula la direcci�n de la c�mara
            camDirection();

            // Mueve el jugador en funci�n de la entrada y la c�mara
            movePlayer = playerInput.x * camRight + playerInput.z * camForward;
            movePlayer *= playerSpeed;

            // Ajusta la direcci�n en la que mira el jugador
            if (playerInput.magnitude > 0) // Solo mira hacia la direcci�n de movimiento si el jugador se est� moviendo
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