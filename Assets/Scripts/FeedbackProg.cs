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

    public void PopUp()
    {
        AnnonceHUB.SetActive(false);
        AnnonceIntermediaire.SetActive(true);    
    }

    public void TeloportToInt()
    {
        AnnonceIntermediaire.SetActive(false);
        Player.position = TeleportPointINT.position;
        Player.rotation = TeleportPointINT.rotation;
        AnnonceZone.SetActive(true);

    }

    public void TeloportToZone()
    {
        AnnonceZone.SetActive(false);
        Player.position = TeleportPointZone.position;
        Player.rotation = TeleportPointZone.rotation;
    }

    //public IEnumerator ContentPopUp()
    //{
        //AnnonceHUB.SetActive(false);
        //yield return new WaitForSeconds(1f);
       // AnnonceIntermediaire.SetActive(true);    
    //}
}
