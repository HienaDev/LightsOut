using UnityEngine;

public class BaloonieExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject balooniePrefab;

    public void BlowBaloonieUp()
    {
        balooniePrefab.SetActive(false);
        explosionPrefab.SetActive(true);
    }
}
