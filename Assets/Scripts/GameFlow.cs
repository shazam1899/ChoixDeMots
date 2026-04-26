using UnityEngine;
using System.Collections;

public class GameFlow : MonoBehaviour
{
    public int progression = 0;
    public float WaitTime = 2f;
    public GameObject minijeu1;
    public bool minijeu1finish = false;
    public GameObject minijeu2;
    public bool minijeu2finish = false;
    public GameObject minijeu3;
    public bool minijeu3finish = false;
    public GameObject final;
    public bool finalfinish;
    public GameObject Thanks;
    public GameObject TxtFinal;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(progression == 0) // Ou a mettre en start si ça se détecte pas puisqu'on reste dans le collider
            {
                Debug.Log("Premier jeu dans ton cul");
                //Lance minigame 1
                if(!minijeu1finish)
                {
                    minijeu1.SetActive(true);
                    //scriptToEnable.enabled = true;
                    minijeu1finish = true;
                }
            }

            if(progression == 1)
            {
                Debug.Log("Deuxième jeu dans ton cul");
                //Lance minigame 2
                if(!minijeu2finish)
                {
                    minijeu2.SetActive(true);
                    //scriptToEnable.enabled = true;
                    minijeu2finish = true;
                }
            }

            if(progression == 2)
            {
                Debug.Log("Troisième jeu dans ton cul");
                //Lance minigame 3
                if(!minijeu3finish)
                {
                    minijeu3.SetActive(true);
                    //scriptToEnable.enabled = true;
                    minijeu3finish = true;
                }
            }

            if(progression == 3)
            {
                Debug.Log("Final play");
                if(!finalfinish)
                {
                    StartCoroutine(LaunchFinal());
                    finalfinish = true;
                }
            }
        }

        IEnumerator LaunchFinal()
        {
            Thanks.SetActive(true);
            yield return new WaitForSeconds(WaitTime);
            TxtFinal.SetActive(true);
            final.SetActive(true);
        }
    }
}

//A mettre au début des 2 premiers scripts de dialogue
//[SerializeField] private GameFlow progression;
//A mettre à la fin des dialogues ou au début 
//progression += 1;