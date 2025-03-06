using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    [SerializeField] private AudioClip[] audios;
    private AudioSource audioSource;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float spatialBlend = 1f;

    [SerializeField] private bool playOnBeggining = true;

    [SerializeField] private bool secondSound = false;
    [SerializeField] private AudioClip[] audio2s;
    private AudioSource audio2Source;
    [SerializeField] private float spatialBlend2 = 1f;
    [SerializeField] private float volume2 = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {

        audio2Source = gameObject.AddComponent<AudioSource>();
        audio2Source.volume = volume2;
        audio2Source.loop = false;
        audio2Source.spatialBlend = spatialBlend2;
        audio2Source.playOnAwake = false;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.spatialBlend = spatialBlend;
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
        if (audios.Length == 0)
            return;
        audioSource.clip = audios[Random.Range(0, audios.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.Play();

        if(secondSound)
        {
            if (audio2s.Length == 0)
                return;
            audio2Source.clip = audio2s[Random.Range(0, audio2s.Length)];
            audio2Source.pitch = Random.Range(0.9f, 1.1f);

            audio2Source.Play();
        }
    }
}
