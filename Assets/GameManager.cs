using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("BulbManager")]
    [SerializeField] private BulbManager bulbManager;
    [SerializeField] private int timeDisplayingBulbs = 10;
    [SerializeField] private int timeShufflingBulbs = 5;

    [SerializeField] private SlapLogic slapLogic;

    [SerializeField] private ItemSwapper bulbShuffler;

    [SerializeField] private int lives = 3;

    [SerializeField] private CameraShakeWithObject cameraShakeWithObject;

    private int currentRound = 0;

    public bool waitingForPress = false;
    public bool press = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartRound(currentRound));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartRound(int round)
    {

        yield return new WaitForSeconds(1f);

        if (round > bulbManager.rounds.Length - 1)
        {
            round = bulbManager.rounds.Length - 1;
        }

        bulbManager.BulbSpawner(round);

        yield return new WaitForSeconds(timeDisplayingBulbs);

        bulbManager.ToggleBulbsLight(false);

        yield return new WaitForSeconds(1f);

        bulbShuffler.ToggleSwapping(true);

        yield return new WaitForSeconds(timeShufflingBulbs);

        bulbShuffler.ToggleSwapping(false);

        yield return new WaitForSeconds(3f);

        cameraShakeWithObject.canStartShaking = true;
        waitingForPress = true;

        while (press == false)
        {
            yield return null;
        }

        cameraShakeWithObject.canStartShaking = false;
        press = false;
        waitingForPress = false;

        bulbManager.ToggleBulbsLight(true);

        yield return new WaitForSeconds(1f);

        if (bulbManager.CheckAliveBulb())
        {
            slapLogic.ActivateSlapAnimation();
            yield return new WaitForSeconds(5f);
            lives -= 1;
        }

        bulbManager.BlowAllBulbs();

        if (lives > 0)
        {
            currentRound += 1;
            StartCoroutine(StartRound(currentRound));
        }
        else
        {
            Debug.Log("Game Over");
        }

    }
}
