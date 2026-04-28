using UnityEngine;
using System.Collections;

public class SateGatherer : MonoBehaviour
{
    [SerializeField] private GameObject ImMule;
    [SerializeField] private GameObject TxtMule;
    [SerializeField] private GameObject ImFaucon;
    [SerializeField] private GameObject TxtFaucon;
    [SerializeField] private GameObject ImParesseux;
    [SerializeField] private GameObject TxtParesseux;
    [SerializeField] private GameObject ImAutruche;
    [SerializeField] private GameObject TxtAutruche;
    [SerializeField] private GameObject ImPigeon;
    [SerializeField] private GameObject TxtPigeon;
    [SerializeField] private GameObject ImSerpent;
    [SerializeField] private GameObject TxtSerpent;
    [SerializeField] private GameObject ImCapybara;
    [SerializeField] private GameObject TxtCapybara;
    [SerializeField] private GameObject ImHamster;
    [SerializeField] private GameObject TxtHamster;
    [SerializeField] private GameObject ImOurs;
    [SerializeField] private GameObject TxtOurs;

    int NumberBlock = 0;
    int NumberName = 0;
    float MeanTime = 0;
    float WeirdTime = 0;

    void Start()
    {
        SuccessBlock();
    }

    public void SuccessBlock()
    {
        NumberBlock = 0;
        NumberName = 0;

        foreach (var item in FindObjectsByType<Bloquer>(FindObjectsSortMode.None))
        {
            if(item.Blocked)
            {
                NumberBlock += 1;
            }
            
            if(!string.IsNullOrEmpty(item.Name))
            {
                NumberName += 1;
            }

            if (item.Name == "mechant")
            {
                MeanTime = item.ActiveTime;
            }

            if (item.Name == "bizarre")
            {
                WeirdTime = item.ActiveTime;
            }
        }

        Debug.Log("NumberBlock = " + NumberBlock);
        Debug.Log("MeanTime = " + MeanTime);
        Debug.Log("WeirdTime = " + WeirdTime);

        if(NumberBlock == 0)
        {
            ImMule.SetActive(true);
            TxtMule.SetActive(true);
            Debug.Log("Mule");
        }
        else if(NumberBlock == 2 && MeanTime < 8 && WeirdTime < 15)
        {
            ImFaucon.SetActive(true);
            TxtFaucon.SetActive(true);
            Debug.Log("Faucon");
        }
        else if(NumberBlock == 2 && MeanTime > 8 && WeirdTime > 15)
        {
            ImParesseux.SetActive(true);
            TxtParesseux.SetActive(true);
            Debug.Log("Paresseux");
        }
        else if (NumberBlock == 3)
        {
            ImAutruche.SetActive(true);
            TxtAutruche.SetActive(true);
            Debug.Log("Autruche");
        }

        foreach (var item in FindObjectsByType<Bloquer>(FindObjectsSortMode.None))
        {
            if (NumberBlock == 1 && item.Name == "mechant")
            {
                ImPigeon.SetActive(true);
                TxtPigeon.SetActive(true);
                Debug.Log("Pigeon");
                break;
            }
            else if (NumberBlock == 1 && item.Name == "gentil")
            {
                ImSerpent.SetActive(true);
                TxtSerpent.SetActive(true);
                Debug.Log("Serpent");
                break;
            }
            else if (NumberBlock == 1 && item.Name == "bizarre")
            {
                ImCapybara.SetActive(true);
                TxtCapybara.SetActive(true);
                Debug.Log("Capybara");
                break;
            }
            else if(NumberBlock == 2 && item.Name == "cancel")
            {
                ImOurs.SetActive(true);
                TxtOurs.SetActive(true);
                Debug.Log("Ours");
            }
            else if(NumberBlock == 3 && item.Name == "cancel")
            {
                ImHamster.SetActive(true);
                TxtHamster.SetActive(true);
                Debug.Log("Hamster");
            }
        }












        //Va trouver les objets du meme type
        //FindObjectsByType<Bloquer>(FindObjectsSortMode.None)[0].Name;
        //FindObjectsByType<Bloquer>(FindObjectsSortMode.None)[0].Blocked;

        //if(Bloquer.Blocked == 2) //&& !chat.MoitiéDialogue)
        //{
            //Debug.Log("Faucon");
        //}

        //else if(Bloquer.Blocked == 2 && chat.MoitiéDialogue)
        //{
            //Debug.Log("Paresseux");
        //}

        //if (Bloquer.Blocked == 0)
        //if(!Blocked)
        //{
            //Debug.Log("Mule");
        //}

        //if (Bloquer.Blocked == 3)
        //{
            //Debug.Log("Autruche");
        //}

        //else if(Bloquer.Blocked == 2 && chat.BlockYou)
        //{
            //Debug.Log("Hamster");
        //}

        //else if(Bloquer.Blocked == 1 && chat.BlockYou)
        //{
            //Debug.Log("Ours");
        //}

        //if (Bloquer.Blocked == 1 && block.Name == "mechant")
        //{
            //Debug.Log("Pigeon");
        //}

        //if (Bloquer.Blocked == 1 && block.Name == "gentil")
        //{
            //Debug.Log("Serpent");
        //}

        //if (Bloquer.Blocked == 1 && block.Name == "bizarre")
        //{
            //Debug.Log("Capybara");
        //}
    }
}
