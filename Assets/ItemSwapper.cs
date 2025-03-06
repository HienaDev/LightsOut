using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwapper : MonoBehaviour
{

    public float arcHeight = 2.0f; // Height of the arc
    public float swapSpeed = 1.0f; // Speed of the swap
    public float minRandomDelay = 1.0f; // Maximum random delay between swaps
    public float maxRandomDelay = 1.0f; // Maximum random delay between swaps

    private bool isSwapping = false; // Boolean to control swapping
    private HashSet<GameObject> currentlySwapping = new HashSet<GameObject>(); // Track items currently swapping





    public void ToggleSwapping(bool toggle)
    {
        isSwapping = toggle;
    }   

    // Method to start swapping items
    public void SwapItemsInList(List<GameObject> items)
    {
        if (items == null || items.Count < 2)
        {
            Debug.LogWarning("Not enough items to swap.");
            return;
        }

        // Start the continuous swapping coroutine
        StartCoroutine(ContinuousSwapping(items));
    }

    // Coroutine to continuously swap items while isSwapping is true
    private IEnumerator ContinuousSwapping(List<GameObject> items)
    {
        while (true)
        {
            // Only perform swaps if isSwapping is true
            if (isSwapping)
            {
                // Create a list of available indices (excluding items currently swapping)
                List<int> availableIndices = new List<int>();
                for (int i = 0; i < items.Count; i++)
                {
                    if (!currentlySwapping.Contains(items[i]))
                    {
                        availableIndices.Add(i);
                    }
                }

                // Perform swaps until no more pairs are available
                while (availableIndices.Count >= 2 && isSwapping)
                {
                    // Randomly select two indices to swap
                    int index1 = availableIndices[Random.Range(0, availableIndices.Count)];
                    availableIndices.Remove(index1);

                    int index2 = availableIndices[Random.Range(0, availableIndices.Count)];
                    availableIndices.Remove(index2);

                    // Mark the items as currently swapping
                    currentlySwapping.Add(items[index1]);
                    currentlySwapping.Add(items[index2]);

                    // Start the swap coroutine for these two items with a random delay
                    StartCoroutine(SwapWithDelay(items[index1], items[index2], Random.Range(minRandomDelay, maxRandomDelay)));
                }
            }

            // Wait for a short time before checking again
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Coroutine to add a random delay before starting the swap
    private IEnumerator SwapWithDelay(GameObject item1, GameObject item2, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the random delay
        StartCoroutine(SwapItems(item1.transform, item2.transform, arcHeight, swapSpeed));
    }

    // Coroutine to swap two items in an arced motion
    private IEnumerator SwapItems(Transform item1, Transform item2, float arcHeight, float swapSpeed)
    {
        Vector3 startPos1 = item1.position;
        Vector3 startPos2 = item2.position;

        float progress = 0.0f;

        // Determine the arc direction for each item
        float arcDirection1 = 1.0f; // Above arc
        float arcDirection2 = -1.0f; // Below arc

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * swapSpeed;

            // Calculate the arc positions
            float arcProgress = Mathf.Sin(progress * Mathf.PI);
            Vector3 arcOffset1 = Vector3.up * arcHeight * arcProgress * arcDirection1;
            Vector3 arcOffset2 = Vector3.up * arcHeight * arcProgress * arcDirection2;

            // Move the items along the arc
            item1.position = Vector3.Lerp(startPos1, startPos2, progress) + arcOffset1;
            item2.position = Vector3.Lerp(startPos2, startPos1, progress) + arcOffset2;

            yield return null;
        }

        // Ensure the items are exactly at their target positions
        item1.position = startPos2;
        item2.position = startPos1;

        // Mark the items as no longer swapping
        currentlySwapping.Remove(item1.gameObject);
        currentlySwapping.Remove(item2.gameObject);
    }
}