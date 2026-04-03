using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapAndLock : MonoBehaviour
{
    public Transform snapPoint;
    public InteractionLayerMask blockLayerMask;

    private void Start()
    {
        // On vérifie si l'objet est un bloc
        {
            // On récupère le script BlockLock sur l'objet
            XRSocketInteractor interactable = GetComponent<XRSocketInteractor>();
                interactable.selectExited.AddListener(OnReleaseEvent);
        }
    }

    private void OnReleaseEvent(SelectExitEventArgs arg0)
    {
        if (arg0.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().interactionLayers == blockLayerMask)
        {
            arg0.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }
}
