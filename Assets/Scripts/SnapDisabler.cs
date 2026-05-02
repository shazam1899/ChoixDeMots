using UnityEngine;

public class SnapDisabler : MonoBehaviour
{
    public GameObject[] snapVolume;
    private bool playerInside = false;

    private int insideCount = 0;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideCount++;

            if (insideCount == 1)
            {
                Debug.Log("bb");
                foreach (var obj in snapVolume)
                    obj?.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideCount--;

            if (insideCount <= 0)
            {
                Debug.Log("donne-moi ton adresse");
                insideCount = 0;

                foreach (var obj in snapVolume)
                    obj?.SetActive(true);
            }
        }
    }

}
