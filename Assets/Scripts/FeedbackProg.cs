using UnityEngine;
using System.Collections;

public class FeedbackProg : MonoBehaviour
{
    [SerializeField] private Transform TeleportPointINT;
    [SerializeField] private Transform TeleportPointZone;
    [SerializeField] private Transform Player;
    [SerializeField] private GameObject AnnonceHUB;
    [SerializeField] private GameObject AnnonceIntermediaire;
    [SerializeField] private GameObject AnnonceZone;
    [SerializeField] private GameObject PermTeleportPointZone;

    [Header("Son de notification")]
    [SerializeField] private AudioClip notificationSound;

    public void PopUp()
    {
        AnnonceHUB.SetActive(false);
        AnnonceIntermediaire.SetActive(true);

        // 🔊 Joue le son de notification
        //NotificationSound.Instance.PlayNotification(notificationSound);
    }

    public void TeloportToInt()
    {
        AnnonceIntermediaire.SetActive(false);
        Player.position = TeleportPointINT.position;
        Player.rotation = TeleportPointINT.rotation;
        AnnonceZone.SetActive(true);

        // 🔊 Joue le son de notification
        //NotificationSound.Instance.PlayNotification(notificationSound);
    }

    public void TeloportToZone()
    {
        AnnonceZone.SetActive(false);
        Player.position = TeleportPointZone.position;
        Player.rotation = TeleportPointZone.rotation;
        PermTeleportPointZone.SetActive(true);

        // 🔊 Joue le son de notification
        //NotificationSound.Instance.PlayNotification(notificationSound);
    }
}

