using UnityEngine;
using System.Collections;

public class FeedbackProg : MonoBehaviour
{
    [SerializeField] private GameObject AnnonceHUB;
    [SerializeField] private GameObject AnnonceIntermediaire;

    public void PopUp()
    {
        AnnonceHUB.SetActive(false);
        AnnonceIntermediaire.SetActive(true);    
    }

    //public IEnumerator ContentPopUp()
    //{
        //AnnonceHUB.SetActive(false);
        //yield return new WaitForSeconds(1f);
       // AnnonceIntermediaire.SetActive(true);    
    //}
}
