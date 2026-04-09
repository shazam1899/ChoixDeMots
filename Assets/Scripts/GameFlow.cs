using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public int progression = 0;
    public bool minijeu1finish = false;
    public bool minijeu2finish = false;
    public bool minijeu3finish = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(progression == 0) // Ou a mettre en start si ça se détecte pas puisqu'on reste dans le collider
            {
                Debug.Log("Premier jeu dans ton cul");
                //Lance minigame 1
                //if(!minijeu1finish)
                //{
                    //scriptToEnable.enabled = true;
                    //minijeu1finish = true;
                //}
            }
            if(progression == 1)
            {
                Debug.Log("Deuxième jeu dans ton cul");
                //Lance minigame 2
                //if(!minijeu2finish)
                //{
                    //scriptToEnable.enabled = true;
                    //minijeu2finish = true;
                //}
            }
            if(progression == 2)
            {
                Debug.Log("Troisième jeu dans ton cul");
                //Lance minigame 3
                //if(!minijeu3finish)
                //{
                    //scriptToEnable.enabled = true;
                    //minijeu3finish = true;
                //}
            }
        }
    }
}

//A mettre au début des 2 premiers scripts de dialogue
//[SerializeField] private GameFlow progression;
//A mettre à la fin des dialogues ou au début 
//progression += 1;