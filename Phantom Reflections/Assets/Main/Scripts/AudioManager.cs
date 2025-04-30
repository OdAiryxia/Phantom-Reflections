using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play()
    {
        audioSource.Play();
        StartCoroutine(WaitForSound());
    }

    private IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.resource = null;
    }
}
