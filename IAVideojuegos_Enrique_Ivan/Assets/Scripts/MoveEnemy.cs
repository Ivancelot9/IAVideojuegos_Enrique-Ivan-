using UnityEngine;
using UnityEngine.AI;

public class MoveEnemy : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Alert,
        Attack
    }

    public State currentState;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float rotationSpeed = 50f; // Velocidad de rotación para patrullaje
    public float attackDuration = 5f; // Duración del ataque
    public float patrolRadius = 20f; // Radio del área de patrullaje aleatorio
    public float patrolWaitTime = 3f; // Tiempo de espera en cada punto de patrullaje

    private Vector3 lastKnownPlayerPosition;
    private bool isPlayerDetected;
    private float patrolTimer;
    private Vector3 patrolTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
            enabled = false; // Desactiva el script si no se encuentra el jugador
            return;
        }
        currentState = State.Patrol;
        patrolTimer = patrolWaitTime; // Inicializar el temporizador de patrullaje
        SetNextPatrolPoint(); // Establece el primer destino de patrullaje
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchState(State.Alert);
            Debug.Log("Estado cambiado a Alerta");
        }

        CheckStateTransitions();

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Alert:
                Alert();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void CheckStateTransitions()
    {
        if (player == null) return; // Asegúrate de que player no sea null

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        isPlayerDetected = distanceToPlayer < detectionRange;

        if (currentState == State.Attack)
        {
            if (Vector3.Distance(transform.position, player.position) < attackRange)
            {
                // El jugador ha sido alcanzado
                HandlePlayerDetection();
            }
            else if (Time.time - attackStartTime > attackDuration)
            {
                // Regresar a patrullaje después de N segundos
                SwitchState(State.Patrol);
                SetNextPatrolPoint();
            }
        }
        else if (distanceToPlayer < attackRange)
        {
            SwitchState(State.Attack);
        }
        else if (distanceToPlayer < detectionRange)
        {
            SwitchState(State.Alert);
        }
        else if (currentState != State.Patrol)
        {
            SwitchState(State.Patrol);
            SetNextPatrolPoint();
        }
    }

    void SwitchState(State newState)
    {
        currentState = newState;
        animator.SetBool("isAlert", newState == State.Alert);
        animator.SetBool("isRunning", newState == State.Attack);

        if (newState == State.Patrol)
        {
            agent.speed = 3.5f; // Velocidad de patrullaje
            patrolTimer = patrolWaitTime; // Reinicia el temporizador de patrullaje
        }
        else if (newState == State.Alert)
        {
            agent.speed = 5f; // Velocidad de alerta
        }
        else if (newState == State.Attack)
        {
            agent.speed = 2f; // Velocidad de ataque (más lenta)
            attackStartTime = Time.time; // Marcar el tiempo de inicio del ataque
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0)
            {
                SetNextPatrolPoint();
                patrolTimer = patrolWaitTime; // Reinicia el temporizador de patrullaje
            }
        }
        else
        {
            RotateAgent();
        }
    }

    void Alert()
    {
        if (isPlayerDetected)
        {
            // Sigue al jugador si está detectado
            agent.SetDestination(player.position);
        }
        else
        {
            // Mueve a la última posición conocida si el jugador no está detectado
            agent.SetDestination(lastKnownPlayerPosition);
        }

        RotateAgent();
    }

    void Attack()
    {
        agent.SetDestination(lastKnownPlayerPosition);
        RotateAgent();
    }

    void RotateAgent()
    {
        // Solo gira el agente si no está en movimiento
        if (!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandlePlayerDetection()
    {
        // Aquí agregas la lógica para manejar la detección del jugador, como desactivarlo o mostrar una animación
        player.gameObject.SetActive(false); // Ejemplo simple: desactiva al jugador
        // Puedes agregar aquí animación, efectos de sonido, etc.
    }

    void SetNextPatrolPoint()
    {
        // Genera un destino aleatorio dentro del radio de patrullaje
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
        }
    }

    // Gizmos para visualizar el cono de visión y el indicador de detección
    private void OnDrawGizmos()
    {
        // Dibuja el cono de visión
        Gizmos.color = GetConeColor();
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 direction = transform.forward * detectionRange;
        Gizmos.DrawLine(transform.position, transform.position + direction);

        // Dibuja el indicador de detección
        Gizmos.color = isPlayerDetected ? Color.red : Color.black;
        Gizmos.DrawSphere(transform.position + transform.forward * detectionRange, 0.5f);
    }

    Color GetConeColor()
    {
        switch (currentState)
        {
            case State.Patrol:
                return Color.green;
            case State.Alert:
                return Color.yellow;
            case State.Attack:
                return Color.red;
            default:
                return Color.white;
        }
    }

    private float attackStartTime;
}
