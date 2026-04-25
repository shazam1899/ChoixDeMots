using UnityEngine;
using System.Collections;

public class SateGatherer : MonoBehaviour
{
    //[SerializeField] private DialogueManager chatMean;
    //[SerializeField] private DialogueManager chatKind;
    //[SerializeField] private DialogueManager chatWeird;
    public GameObject chatMean;
    public GameObject chatWeird;
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

        if(NumberBlock == 0)
        {
            Debug.Log("Mule");
        }

        if(NumberBlock == 2 && MeanTime < 8 && WeirdTime < 15)
        {
            Debug.Log("Faucon");
        }

        if(NumberBlock == 2 && MeanTime > 8 && WeirdTime > 15)
        {
            Debug.Log("Paresseux");
        }

        if (NumberBlock == 3)
        {
            Debug.Log("Autruche");
        }

        foreach (var item in FindObjectsByType<Bloquer>(FindObjectsSortMode.None))
        {
            if (NumberBlock == 1 && item.Name == "mechant")
            {
                Debug.Log("Pigeon");
                break;
            }

            if (NumberBlock == 1 && item.Name == "gentil")
            {
                Debug.Log("Serpent");
                break;
            }

            if (NumberBlock == 1 && item.Name == "bizarre")
            {
                Debug.Log("Capybara");
                break;
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
