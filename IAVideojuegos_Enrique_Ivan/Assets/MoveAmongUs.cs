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

    // Variable Gravedad
    public float gravity = 9.8f;
    public float fallVelocity;

    // Variable Salto
    public float jumpForce;
   



    void Start()
    {
        player = GetComponent<CharacterController>();
        // Asignar la cámara principal si no está asignada en el inspector
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

    }

    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);
        camDirection();
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * playerSpeed;
        player.transform.LookAt(player.transform.position + movePlayer);
        SetGravity();
        SaltoPersonaje();
        player.Move(movePlayer * Time.deltaTime);
    }

    void camDirection()
    {
        if (mainCamera == null) return; // Si no hay cámara asignada, salir de la función
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
        }
    }

    private void SaltoPersonaje()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}


