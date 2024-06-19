using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSenses : MonoBehaviour
//canal de youtube para un poco de programaci�n de unity https://www.youtube.com/c/SebastianLague
//Se utilizo chat para la busqueda de informaci�n.

{
    public float VisionRange = 10.0f; // Rango de visi�n del agente
    public float VisionAngle = 45.0f; // �ngulo de visi�n en grados
    public Transform InfiltratorTransform = null; // Referencia al transform del infiltrador

    void Start()
    {
        TestVision(); // Llama a la funci�n para probar la visi�n al iniciar
    }

    void Update()
    {
        CheckVision(InfiltratorTransform.position); // Verifica la visi�n del infiltrador en cada frame
    }

    void TestVision()
    {
        Vector3 testPosition = new Vector3(1, 2, 3); // Posici�n de prueba
        bool canSee = IsTargetVisible(testPosition); // Verifica si puede ver la posici�n de prueba

        if (canSee)
        {
            Debug.Log("Lo veo"); // Si puede ver la posici�n de prueba, imprime "Lo veo"
        }
        else
        {
            Debug.Log("No lo veo"); // Si no puede ver la posici�n de prueba, imprime "No lo veo"
        }
    }

    // Verifica si el objetivo es visible (dentro del rango y �ngulo de visi�n)
    bool IsTargetVisible(Vector3 targetPosition)
    {
        if (!TargetIsInRange(targetPosition))
            return false;

        return IsWithinVisionCone(targetPosition);
    }

    // Verifica si el objetivo est� dentro del rango de visi�n
    public bool TargetIsInRange(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        return distance <= VisionRange;
    }

    // Verifica si el objetivo est� dentro del �ngulo de visi�n
    bool IsWithinVisionCone(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        return angleToTarget <= VisionAngle / 2;
    }

    // Verifica si el objetivo est� tanto en el rango como en el �ngulo de visi�n
    public bool TargetIsInVisionCone(Vector3 targetPosition)
    {
        return TargetIsInRange(targetPosition) && IsWithinVisionCone(targetPosition);
    }

    // Verifica la visi�n del objetivo y imprime un mensaje seg�n el resultado
    void CheckVision(Vector3 targetPosition)
    {
        if (IsTargetVisible(targetPosition))
        {
            Debug.Log("Lo veo");
        }
        else
        {
            Debug.Log("No lo veo");
        }
    }

    // Dibuja Gizmos en la escena para visualizar el rango y el �ngulo de visi�n del agente
    void OnDrawGizmos()
    {
        // Dibuja un c�rculo para representar el rango de visi�n
        Gizmos.color = TargetIsInRange(InfiltratorTransform.position) ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, VisionRange);

        // Dibuja las l�neas del �ngulo de visi�n
        Vector3 forward = transform.forward * VisionRange;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-VisionAngle / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(VisionAngle / 2, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftRayDirection);
        Gizmos.DrawRay(transform.position, rightRayDirection);

        // Dibuja una l�nea hacia adelante para representar la direcci�n del agente
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * VisionRange);
    }
}
