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

    private XRSocketInteractor socket;

    private void Start()
    {
        // On récupère le socket interactor sur cet objet
        socket = GetComponent<XRSocketInteractor>();

        if (socket != null)
        {
            socket.selectExited.AddListener(OnReleaseEvent);
        }
        else
        {
            Debug.LogWarning("SnapAndLock : Aucun XRSocketInteractor trouvé sur cet objet.", this);
        }
    }

    private void OnReleaseEvent(SelectExitEventArgs args)
    {
        // Vérifie que l'objet relâché est bien un XRGrabInteractable
        XRGrabInteractable grab = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        if (grab == null)
            return;

        // Vérifie que l'objet appartient bien au layer des blocs
        if (grab.interactionLayers != blockLayerMask)
            return;

        // Désactive le grab pour verrouiller l'objet
        grab.enabled = false;

        // Place l'objet exactement au snapPoint
        args.interactableObject.transform.position = snapPoint.position;
        args.interactableObject.transform.rotation = snapPoint.rotation;

        // Met à jour le score
        LeaderboardManager leaderboard = FindFirstObjectByType<LeaderboardManager>();
        if (leaderboard != null)
        {
            foreach (var update in scoreUpdates)
            {
                leaderboard.UpdatePlayerScore(update.playerName, update.scoreValue);
            }
        }
        else
        {
            Debug.LogWarning("SnapAndLock : Aucun LeaderboardManager trouvé dans la scène.");
        }
    }
}
