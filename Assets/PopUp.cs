using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject canvas;
    [SerializeField] List<AudioClip> meowSound;

    private void Start()
    {
        menu.SetActive(false);
    }

    private void Update()
    {
        canvas.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            menu.SetActive(true);
            menu.transform.localScale = Vector3.zero;
            menu.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            SoundManager.instance.PlaySFXRandom(meowSound, transform.position, 1f);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            menu.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo).OnComplete(Deactivate);
        }
    }


    public void Deactivate()
    {
        menu.SetActive(false );
    }

}
