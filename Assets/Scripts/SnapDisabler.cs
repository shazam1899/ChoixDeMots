using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapDisabler : MonoBehaviour
{
    public XRInteractableSnapVolume Snapcript;
    public Collider CollideDis;

    private void Awake()
    {
        Snapcript.enabled = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && collider == CollideDis)
        {
            Debug.Log("Hiiiii");
            Snapcript.enabled = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player") && collider == CollideDis)
        {
            Debug.Log("Byeeee");
            Snapcript.enabled = true;
        }
    }
}
