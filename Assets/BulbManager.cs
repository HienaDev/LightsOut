using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class BulbManager : MonoBehaviour
{
    //[SerializeField] private int numberOfBulbs = 3;
    //[SerializeField] private int numberOfBadBulbs = 1;
    [SerializeField] private GameObject bulbPrefab;
    [SerializeField] private Transform initialPosition;
    [SerializeField] private Transform playerTransform; // Center of radial layout
    [SerializeField] private float radius = 5f; // Distance from player
    [SerializeField] private float bulbDistance = 2f; // Minimum distance between bulbs

    [SerializeField] private CameraShakeWithObject cameraShakeWithObject;

    private List<GameObject> instantiatedBulbs;
    private GameObject currentlySelectedBulb;
    public int bulbIndex;

    private ItemSwapper itemSwapper;

    private int currentRound = 0;

    [Serializable]
    public struct Round
    {
        public int numberOfBulbs;
        public int numberOfBadBulbs;
    }

    [SerializeField] private Round[] rounds;

    void Start()
    {
        itemSwapper = GetComponent<ItemSwapper>();
        instantiatedBulbs = new List<GameObject>();

        BulbSpawner(7, 2);
        //StartCoroutine(TestRoundsCR());
    }

    private void Update()
    {

    }

    private IEnumerator TestRoundsCR()
    {
        for (int i = 0; i < rounds.Length; i++)
        {
            yield return new WaitForSeconds(5f);
            BulbSpawner(rounds[i].numberOfBulbs, rounds[i].numberOfBadBulbs);
        }
    }


    public void BulbSpawner(int numberOfBulbs, int numberOfBadBulbs)
    {

        foreach (GameObject bulb in instantiatedBulbs)
        {
            Destroy(bulb);
        }

        instantiatedBulbs.Clear();



        //// Calculate offset to center bulbs
        //float centerOffset = (numberOfBulbs - 1) * 0.5f * bulbDistance;

        //for (int i = 0; i < numberOfBulbs; i++)
        //{
        //    GameObject bulb = Instantiate(bulbPrefab);
        //    instantiatedBulbs.Add(bulb);
        //    bulb.GetComponent<Bulb>().bulbIndex = i;
        //    float zPosition = initialPosition.position.z + (i * bulbDistance - centerOffset);
        //    bulb.transform.position = new Vector3(initialPosition.position.x, initialPosition.position.y, zPosition);
        //}

        float angleStep = bulbDistance / radius; // Angle step based on distance and radius
        float startAngle = -((numberOfBulbs - 1) * angleStep) / 2; // Center bulbs around player


        HashSet<int> badBulbIndices = new HashSet<int>();

        int attempts = 0;
        while (badBulbIndices.Count < numberOfBadBulbs && attempts < 100)
        {
            badBulbIndices.Add(UnityEngine.Random.Range(0, numberOfBulbs));
            attempts++;
        }

        if(attempts >= 100)
        {
            Debug.LogError("Could not find enough unique bad bulb indices");
            return;
        }

        for (int i = 0; i < numberOfBulbs; i++)
        {
            float angle = startAngle + (i * angleStep); // Angle for this bulb

            float xPos = playerTransform.position.x + radius * Mathf.Cos(angle);
            float zPos = playerTransform.position.z + radius * Mathf.Sin(angle);

            GameObject bulb = Instantiate(bulbPrefab);
            instantiatedBulbs.Add(bulb);

            if(badBulbIndices.Contains(i))
                bulb.GetComponent<Renderer>().material.color = Color.red;
            else
                bulb.GetComponent<Renderer>().material.color = Color.blue;

            bulb.GetComponent<Bulb>().bulbIndex = i;
            bulb.transform.position = new Vector3(xPos, initialPosition.position.y, zPos); // Keep Z fixed
        }

        itemSwapper.SwapItemsInList(instantiatedBulbs);

    }

    public void SelectBulb(int index)
    {


        if (currentlySelectedBulb != null)
            currentlySelectedBulb.GetComponent<Renderer>().material.color = Color.white;

        bulbIndex = index;

        if (index == -1)
        {
            cameraShakeWithObject.SetObject(null);
            return;
        }

        currentlySelectedBulb = instantiatedBulbs[bulbIndex];
        cameraShakeWithObject.SetObject(currentlySelectedBulb);

        if (currentlySelectedBulb != null)
            currentlySelectedBulb.GetComponent<Renderer>().material.color = Color.red;
    }
}
