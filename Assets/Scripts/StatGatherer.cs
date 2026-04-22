using UnityEngine;
using System.Collections;

public class SateGatherer : MonoBehaviour
{
    [SerializeField] private DialogueManager chat;
    int NumberBlock = 0;
    int NumberName = 0;

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
        }

        if(NumberBlock == 0)
        {
            Debug.Log("Mule");
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
