using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public class PlayerScoreUpdate
{
    public string playerName;
    public int scoreValue;
}

public class SnapAndLock : MonoBehaviour
{
    public Transform snapPoint;
    public InteractionLayerMask blockLayerMask;
    public List<PlayerScoreUpdate> scoreUpdates;


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
            
                foreach (var update in scoreUpdates)
                {
                    FindFirstObjectByType<LeaderboardManager>().UpdatePlayerScore(update.playerName, update.scoreValue);
                }
            
        }
    }
}
