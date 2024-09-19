using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppluForceOnCillision : MonoBehaviour
{
    public float forceMagnitude = 5;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if(rb != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            Vector3 forceFirectionNorm = forceDirection.normalized;
            Debug.Log(forceFirectionNorm); 
            if (forceFirectionNorm.y > -0.2f)
            {
                forceDirection.y = 0;
                forceDirection.Normalize();
                rb.AddForceAtPosition(forceDirection * forceMagnitude * Time.deltaTime, transform.position, ForceMode.Impulse);
            }
            
        }
    }
}
