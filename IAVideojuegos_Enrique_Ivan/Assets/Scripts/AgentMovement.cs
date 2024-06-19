using System.Collections;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    //En el tutorial de Unity "IA b�sica en Unity" https://learn.unity.com/course/artificial-intelligence-for-beginners
    //Se utilizo chat para la busqueda de informaci�n.
    public float viewDistance = 10f; // Distancia del cono de visi�n
    public float viewAngle = 45f; // �ngulo del cono de visi�n
    public LayerMask targetMask; // M�scara para los objetivos que el guardia puede ver
    public LayerMask obstacleMask; // M�scara para los obst�culos que pueden bloquear la visi�n
    public float rotationInterval = 5f; // Intervalo de tiempo entre cada rotaci�n
    public float rotationAngle = 45f; // �ngulo de rotaci�n
    private Transform infiltrator; // Referencia al infiltrador
    private bool isAlert = false; // Estado de alerta del guardia

    void Start()
    {
        // Encuentra el infiltrador en la escena usando su etiqueta
        infiltrator = GameObject.FindWithTag("Infiltrator")?.transform;
        if (infiltrator == null)
        {
            Debug.LogError("Infiltrator no encontrado. Aseg�rate de que el infiltrador tenga la etiqueta 'Infiltrator'.");
        }
        else
        {
            // Inicia la rutina de rotaci�n del guardia
            StartCoroutine(RotateGuard());
        }
    }

    void Update()
    {
        if (infiltrator != null && CanSeeTarget(infiltrator))
        {
            isAlert = true; // El guardia entra en estado de alerta
            // Aqu� puedes agregar comportamiento adicional para el estado de alerta
            Debug.Log("Guardia en estado de alerta!");
        }
    }

    IEnumerator RotateGuard()
    {
        while (true)
        {
            // Espera el intervalo de rotaci�n
            yield return new WaitForSeconds(rotationInterval);
            if (!isAlert)
            {
                // Rota el guardia si no est� en estado de alerta
                transform.Rotate(Vector3.up, rotationAngle);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Dibuja los l�mites del cono de visi�n
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        // Dibuja un arco para representar el cono de visi�n
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, viewDistance);
    }

    // Funci�n para verificar si el guardia puede ver al objetivo
    public bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        Vector3 dirToTarget = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
        {
            float dstToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
            {
                return true; // El objetivo est� dentro del cono de visi�n y no hay obst�culos
            }
        }
        return false; // El objetivo est� fuera del cono de visi�n o hay obst�culos
    }
}
