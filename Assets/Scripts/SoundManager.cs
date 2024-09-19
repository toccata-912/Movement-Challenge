using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        
    }

    public void PlaySFXClip(AudioClip audioClip,Transform spawnTransform,float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject,spawnTransform.position,Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLenth = audioSource.clip.length;
        Destroy(audioSource.gameObject,clipLenth);

    }

    public void PlaySFXClipOnLoop(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.transform.SetParent(spawnTransform, true);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySFXRandom(List<AudioClip> audioClip, Vector3 spawnPosition, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        audioSource.clip = audioClip[Random.Range(0,audioClip.Count)];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLenth = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLenth);
    }
}
