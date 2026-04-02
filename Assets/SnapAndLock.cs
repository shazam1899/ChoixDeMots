using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapAndLock : MonoBehaviour
{
    public Transform snapPoint;

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
        Debug.Log(arg0.interactableObject.transform.gameObject);
        arg0.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        //throw new NotImplementedException();
    }


    
}
