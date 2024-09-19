using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollisions : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    private void Update()
    {
        if((transform.position - Player.transform.position).normalized.y > 0.1f)
        {
            collider.isTrigger = true;
            Debug.Log("TOP");
        }
        else
        {
            collider.isTrigger = false;
            Debug.Log("DOWN");
        }
    }
}
