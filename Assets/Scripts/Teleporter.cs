using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] GameObject targetObject = null;
    [SerializeField] Vector3 targetPosition = Vector3.zero;

    public void Teleport()
    {
        targetObject.transform.position = targetPosition;
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawLine(transform.position, targetPosition);
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
    }
}
