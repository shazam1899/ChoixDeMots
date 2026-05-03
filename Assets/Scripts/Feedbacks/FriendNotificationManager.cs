using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;

public class FriendNotificationManager : MonoBehaviour
{
    public static FriendNotificationManager Instance;

    [Header ("UI Elements")]
    public GameObject notificationUI; //appears near player, has arrow
    public GameObject teleporterUI; // appears at teleporter
    public GameObject npcMeanUI; //appears near NPC
    public GameObject npcKindUI; //appears near NPC
    public GameObject npcWeirdUI; //appears near NPC

    [Header ("Arrow")]
    public RectTransform arrowImage; //arrow inside NotificationUI

    [Header ("References")]
    public Transform teleporterPoint; //positon of teleporter
    public Transform npcMeanPoint; //position of NPC
    public Transform npcKindPoint; //position of NPC
    public Transform npcWeirdPoint; //position of NPC
    public Transform playerTransform; //assign camera

    [Header ("Text")]
    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI teleporterText;
    public TextMeshProUGUI npcMeanText;
    public TextMeshProUGUI npcKindText;
    public TextMeshProUGUI npcWeirdText;

    [Header ("Settings")]
    public float notificationDelay = 3f; //seconds after leaderboard updates
    public float teleporterRadius = 2f; //how close to teleporte to trigger
    public float npcRadius = 2f;

    private bool notificationActive = false;
    private bool playerReachedTeleporter = false;
    private string friendName = "";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //hide ui at start
        notificationUI.SetActive(false);
        teleporterUI.SetActive(false);
        npcMeanUI.SetActive(false);
        npcKindUI.SetActive(false);
        npcWeirdUI.SetActive(false);
    }

    private void Update()
    {
        if (!notificationActive) return;

        //rotate arrow to point toward teleporter
        if (arrowImage != null && playerTransform != null && teleporterPoint != null)
        {
            Vector3 direction = teleporterPoint.position - playerTransform.position;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            arrowImage.rotation = Quaternion.Euler(0, 0, -angle);
        }

        //check if player reached teleporter area
        if (!playerReachedTeleporter)
        {
            float distToTeleporter = Vector3.Distance(playerTransform.position, teleporterPoint.position);

            if (distToTeleporter < teleporterRadius)
            {
                OnPlayerReachedTeleporter();
            }
        }
        else
        {
            //check if player reached npc
            float distToNPCMean = Vector3.Distance(playerTransform.position, npcMeanPoint.position);

            if (distToNPCMean < npcRadius)
            {
                OnPlayerReachedNPC();
            }

            //check if player reached npc
            float distToNPCKind = Vector3.Distance(playerTransform.position, npcKindPoint.position);

            if (distToNPCKind < npcRadius)
            {
                OnPlayerReachedNPC();
            }

            //check if player reached npc
            float distToNPCWeird = Vector3.Distance(playerTransform.position, npcWeirdPoint.position);

            if (distToNPCWeird < npcRadius)
            {
                OnPlayerReachedNPC();
            }
        }
    }

    //called by leaderboardManager after update
    public void TriggerFriendNotification(string npcName)
    {
        friendName = npcName;
        StartCoroutine(ShowNotificationAfterDelay());
    }

    private IEnumerator ShowNotificationAfterDelay()
    {
        yield return new WaitForSeconds(notificationDelay);

        //show notification with arrow
        notificationActive = true;
        notificationUI.SetActive(true);
        teleporterUI.SetActive(true);

        if (notificationText != null)
            notificationText.text = $"Nouvel ami ajouté !";
        if (teleporterPoint != null)
            teleporterText.text = $"{friendName} t'as ajouté !";
    }

    private void OnPlayerReachedTeleporter()
    {
        playerReachedTeleporter = true;

        //hide notification, show NPC UI
        notificationUI.SetActive(false);
        
        npcMeanUI.SetActive(true);
        npcKindUI.SetActive(true);
        npcWeirdUI.SetActive(true);
        if (npcMeanText != null)
            npcMeanText.text = $"{friendName} t'as ajouté comme ami !";
        
        if (npcKindText != null)
            npcKindText.text = $"{friendName} t'as ajouté comme ami !";

        if (npcWeirdText != null)
            npcWeirdText.text = $"{friendName} t'as ajouté comme ami !";
    }

    private void OnPlayerReachedNPC()
    {
        //hide everything
        notificationActive = false;
        playerReachedTeleporter = false;
        teleporterUI.SetActive(false);
        npcMeanUI.SetActive(false);
        npcKindUI.SetActive(false);
        npcWeirdUI.SetActive(false);
        friendName = "";
    }

}
