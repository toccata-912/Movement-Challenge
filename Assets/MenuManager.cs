using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject levels;

    [SerializeField] GameObject blackImage;

    [SerializeField] Vector3 outScreen;
    [SerializeField] Vector3 inScreen;

    Tween tweenIn;
    Tween tweenOut;




    private void Start()
    {
        mainMenu.SetActive(true);
        levels.SetActive(false);
    }

    public void GoLevels()
    {
        levels.SetActive(true);
        {
            if (tweenIn != null)
            {
                tweenIn.Kill();
            }
            levels.transform.localPosition = outScreen;
            tweenIn = levels.transform.DOLocalMove(inScreen, 0.7f);
        }

        if (tweenOut != null)
        {
            tweenOut.Kill();
        }
        tweenOut = mainMenu.transform.DOLocalMove(outScreen, 0.7f).OnComplete(DecativateMenu);
    }


  

    public void LevelOne()
    {
        StartCoroutine(LoadScene(1));
    }

    public void LevelTwo()
    {
        StartCoroutine(LoadScene(2));
    }

    IEnumerator LoadScene(int i)
    {
        blackImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(i);

    }

    public void GoMenu()
    {
        mainMenu.SetActive(true);
        {
            if (tweenIn != null) {
                tweenIn.Kill();
            }
            mainMenu.transform.localPosition = outScreen;
            tweenIn = mainMenu.transform.DOLocalMove(inScreen, 0.7f);
        }

        if (tweenOut != null) { 
            tweenOut.Kill();
        }
        tweenOut = levels.transform.DOLocalMove(outScreen, 0.7f).OnComplete(DeactivateLevels);


    }

    public void QuitGame()
    {
        Application.Quit();
    }


    void DecativateMenu()
    {
        mainMenu.SetActive(false);
    }

    void DeactivateLevels()
    {
        levels.SetActive(true);
    }
}
