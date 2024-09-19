using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTregers : MonoBehaviour
{

    [SerializeField] List<GameObject> tregers = new List<GameObject>();
    [SerializeField] Dictionary<GameObject,Vector3> TriggersAndInit = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        foreach(var treger in tregers)
        {
            TriggersAndInit.Add(treger,treger.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(KeyValuePair<GameObject,Vector3> keyValuePair in TriggersAndInit)
            {
                keyValuePair.Key.transform.position = keyValuePair.Value;
            }
        }
    }

}
