using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    [SerializeField] GameObject nextLevel;
    private void OnTriggerEnter(Collider other)
    {
        nextLevel.SetActive(true);
        StartCoroutine(LoadNextSceneAfter());
    }

    IEnumerator LoadNextSceneAfter()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }

}
