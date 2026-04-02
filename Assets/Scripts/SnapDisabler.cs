using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapDisabler : MonoBehaviour
{

    public XRInteractableSnapVolume Snapcript;

    private void Awake()
    {
        Snapcript.enabled = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("Hiiiii");
            Snapcript.enabled = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("Byeeee");
            Snapcript.enabled = true;
        }
    }

}
