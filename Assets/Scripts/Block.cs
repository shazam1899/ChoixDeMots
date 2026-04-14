using UnityEngine;
using System.Collections;

public class Bloquer : MonoBehaviour
{
    public GameObject Avatar;
    public GameObject ChatCanva;

    [SerializeField] private GameObject[] PropsAvatar;
    [SerializeField] private GameObject[] PropsHere;

    //public void LanceEffect()
    //{
        //StartCoroutine(BloqueEffect());
    //}

    public void BloqueEffect()
    {
        //PlayAnimation dispartion
        Avatar.SetActive(false);
        Debug.Log("Je");
        ChatCanva.SetActive(false);
        Debug.Log("suis");

        for (int i = 0; i < PropsAvatar.Length; i++)
        {
            PropsAvatar[i].SetActive(false);
            Debug.Log("derrière");
        }

        //yield return new WaitForSeconds(2f);
        
        for (int i = 0; i < PropsHere.Length; i++)
        {
            PropsHere[i].SetActive(true);
            Debug.Log("toi");
        }
    }
}
