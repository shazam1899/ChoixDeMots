using UnityEngine;

public class SnapDisabler : MonoBehaviour
{
    public GameObject snapVolume;
    private bool playerInside = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInside = true;
            snapVolume.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInside = false;
            snapVolume.SetActive(true);
        }
    }

}
