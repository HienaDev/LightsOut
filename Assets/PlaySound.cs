using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    [SerializeField] private AudioClip[] audios;
    private AudioSource audioSource;
    [SerializeField] private float volume = 1f;

    [SerializeField] private bool playOnBeggining = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        yield return null;

        if (playOnBeggining)
            SoundDo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SoundDo()
    {
        audioSource.clip = audios[Random.Range(0, audios.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.Play();
    }
}
