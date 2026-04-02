using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapDisabler : MonoBehaviour
{
    public GameObject SnapVolume;
    public Collider CollideDis;

    private void Awake()
    {
        SnapVolume.SetActive(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && collider == CollideDis)
        {
            Debug.Log("Hiiiii");
            SnapVolume.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player") && collider == CollideDis)
        {
            Debug.Log("Byeeee");
            SnapVolume.SetActive(true);
        }
    }
}
