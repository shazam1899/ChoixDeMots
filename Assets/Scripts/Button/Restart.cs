using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void Relance()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
