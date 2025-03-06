using System.Collections;

using TMPro;

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

    private int numberOfBulbsBroken;

    [SerializeField] private GameObject gameOverArm;

    [SerializeField] private GameObject bubbleExplosion;

    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private GameObject bubbleTextObject;
    // a
    private string tooManyBroken = "<size=150%>BULBB!</size>\n<color=black>(Greedy hands grasp too much. Restrain yourself.)</color>";
    private string tooFewBroken = "<size=150%>BULBB!</size>\n<color=black>(Hesitation serves no one. Break what must be broken.)</color>";
    private string roundStart = "<size=150%>BULBB!</size>\n<color=black>(The lights go out. Shadows dance. Watch closely… or suffer.)</color>";
    private string shuffling = "<size=150%>BULBB!</size>\n<color=black>(LIGHTS OUT!\nA twist, a turn, a shuffle most cruel… Can you follow? Or shall the dark make a fool of you?)</color>";
    private string choose = "<size=150%>BULBB!</size>\n<color=black>(Now, break the false, leave the true… but choose poorly, and I shall choose you.)</color>";
    private string correctChoice = "<size=150%>BULBB!</size>\n<color=black>(Ah… a keen eye. Perhaps you are not lost to the dark just yet.)</color>";
    private string breakTooManyOrFew = "<size=150%>BULBB!</size>\n<color=black>(Clumsy. Careless. A mind like a flickering bulb…)</color>";
    private string failureSlap = "<size=150%>BULBB!</size>\n<color=black>(Fool! SLAP! That was not the way. Again!)</color>";
    private string tooManySlapsDeath = "<size=150%>BULBB!</size>\n<color=black>(POP! A fragile thing… and so easily undone. The dark swallows all, even you.\")</color>";
    private string finalSuccess = "<size=150%>BULBB!</size>\n<color=black>(Go on… do it. The light is gone. I am but a bubble waiting to burst…)</color>";

    [SerializeField] private GameObject baloonie;
    [SerializeField] private GameObject baloonieFinalDestination;
    [SerializeField] private float durationTravel = 5f;

    [SerializeField] private PlaySound bubbleSound;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }



    public void StartGame()
    {
        StartCoroutine(StartRound(currentRound));
    }

    public void BreakBulb()
    {
        numberOfBulbsBroken += 1;
    }

    private IEnumerator StartRound(int round)
    {
 
        numberOfBulbsBroken = 0;
        yield return new WaitForSeconds(1f);

        bubbleTextObject.SetActive(false);
        bubbleSound.SoundDo();
        yield return null;
        
        bubbleTextObject.SetActive(true);
        bubbleText.text = roundStart;
        if (round > bulbManager.rounds.Length - 1)
        {
            StartCoroutine(EndSequence());
            yield break;
        }

        bulbManager.BulbSpawner(round);

        yield return new WaitForSeconds(timeDisplayingBulbs);

        bulbManager.ToggleBulbsLight(false);

        yield return new WaitForSeconds(1f);

        bubbleTextObject.SetActive(false);
        bubbleSound.SoundDo();
        yield return null;
        
        bubbleTextObject.SetActive(true);

        bubbleText.text = shuffling;

        bulbShuffler.ToggleSwapping(true);

        yield return new WaitForSeconds(timeShufflingBulbs);

        bulbShuffler.ToggleSwapping(false);

        yield return new WaitForSeconds(3f);

        bubbleTextObject.SetActive(false);
        bubbleSound.SoundDo();
        yield return null;
        
        bubbleTextObject.SetActive(true);

        bubbleText.text = choose;

        cameraShakeWithObject.canStartShaking = true;
        waitingForPress = true;

        while (press == false)
        {
            if(numberOfBulbsBroken > bulbManager.rounds[round].numberOfBadBulbs)
            {
                bubbleTextObject.SetActive(false);
                bubbleSound.SoundDo();
                yield return null;
                bubbleSound.SoundDo();
                bubbleTextObject.SetActive(true);

                bubbleText.text = tooManyBroken;
            }
            yield return null;
        }

        if (numberOfBulbsBroken < bulbManager.rounds[round].numberOfBadBulbs)
        {
            bubbleTextObject.SetActive(false);
            bubbleSound.SoundDo();
            yield return null;
            
            bubbleTextObject.SetActive(true);

            bubbleText.text = tooFewBroken;
        }

        cameraShakeWithObject.canStartShaking = false;
        press = false;
        waitingForPress = false;

        bulbManager.ToggleBulbsLight(true);

        yield return new WaitForSeconds(5f);

        if (bulbManager.CheckAliveBulb() || numberOfBulbsBroken > bulbManager.rounds[round].numberOfBadBulbs)
        {

            slapLogic.ActivateSlapAnimation();

            yield return new WaitForSeconds(2f);

            bubbleTextObject.SetActive(false);
            bubbleSound.SoundDo();
            yield return null;
            
            bubbleTextObject.SetActive(true);

            bubbleText.text = failureSlap;
            yield return new WaitForSeconds(3f);
            lives -= 1;
        }
        else
        {
            bubbleTextObject.SetActive(false);
            bubbleSound.SoundDo();
            yield return null;
            
            bubbleTextObject.SetActive(true);

            bubbleText.text = correctChoice;
        }

            bulbManager.BlowAllBulbs(); 
        yield return new WaitForSeconds(3f);
        if (lives > 0)
        {
            currentRound += 1;
            StartCoroutine(StartRound(currentRound));
        }
        else
        {
            bubbleTextObject.SetActive(false);
            bubbleSound.SoundDo();
            yield return null;
            
            bubbleTextObject.SetActive(true);

            bubbleText.text = tooManySlapsDeath;

            gameOverArm.SetActive(true);
            yield break;
        }

    }

    private IEnumerator EndSequence()
    {
        bubbleTextObject.SetActive(false);
        bubbleSound.SoundDo();
        yield return null;
        
        bubbleTextObject.SetActive(true);

        bubbleText.text = finalSuccess;

        StartCoroutine(MoveToPosition(baloonie, baloonieFinalDestination.transform.position, durationTravel));
    }

    private IEnumerator MoveToPosition(GameObject baloonie, Vector3 targetPos, float duration)
    {

        float elapsedTime = 0f; // Track the elapsed time
        Vector3 startPos = baloonie.transform.position; // Store the starting position

        // Loop until the elapsed time reaches the duration
        while (elapsedTime < duration)
        {
            // Interpolate the position based on the elapsed time
            baloonie.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the exact target position
        baloonie.transform.position = targetPos;
        cameraShakeWithObject.canStartShaking = true;
        baloonie.GetComponent<Collider>().enabled = true;

        GameObject bubbleCloneExplosion = Instantiate(bubbleExplosion);
        bubbleCloneExplosion.transform.position = bubbleTextObject.transform.position;
        Destroy(bubbleTextObject,0.1f);
    }
}
