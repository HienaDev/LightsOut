using UnityEngine;

public class BloodSounds : MonoBehaviour
{

    [SerializeField] private AudioClip[] audioBloodSplats;
    private AudioSource audioSourceBlood;
    [SerializeField] private float bloodVolume = 1f;

    [SerializeField] private AudioClip[] audioHeadSmashSplats;
    private AudioSource audioSourceHeadSmash;
    [SerializeField] private float headSmashVolume = 1f;

    [SerializeField] private GameObject restartMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        audioSourceHeadSmash = gameObject.AddComponent<AudioSource>();
        audioSourceHeadSmash.volume = headSmashVolume;
        audioSourceHeadSmash.loop = false;
        audioSourceHeadSmash.playOnAwake = false;

        audioSourceBlood = gameObject.AddComponent<AudioSource>();
        audioSourceBlood.volume = bloodVolume;
        audioSourceBlood.loop = false;
        audioSourceBlood.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BloodSplat()
    {
        audioSourceBlood.clip = audioBloodSplats[0];
        audioSourceBlood.pitch = Random.Range(0.9f, 1.1f);
        audioSourceBlood.Play();
    }

    public void HeadSmash()
    {
        audioSourceHeadSmash.clip = audioHeadSmashSplats[0];
        audioSourceHeadSmash.pitch = Random.Range(0.9f, 1.1f);
        audioSourceHeadSmash.Play();
    }

    public void ShowRestartMenu()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
        restartMenu.SetActive(true);
    }
}
