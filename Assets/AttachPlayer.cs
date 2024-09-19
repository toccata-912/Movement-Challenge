using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject AttachTo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.transform.parent = AttachTo.transform;
            Debug.Log("I am here");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.transform.parent = null;
        }
    }
}
