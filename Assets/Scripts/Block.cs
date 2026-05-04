using UnityEngine;
using System.Collections;

public class Bloquer : MonoBehaviour
{
    public GameObject Self;
    public GameObject Avatar;
    public GameObject ChatCanva;
    public GameObject CheckPanel;
    public GameObject VFXBlock;
    public GameObject AudioAvatar;
    public GameObject AudioHUB;
    public bool Blocked = false;
    public string Name;
    public float ActiveTime;
    public AutoTeleport HUBTel;
    public DialogueManager dialogueManager;
    public GameObject username;

    [SerializeField] private GameObject[] PropsAvatar;
    [SerializeField] private GameObject[] PropsDistorion;
    [SerializeField] private GameObject[] PropsHere;
    private float StartTime;

    private void OnEnable()
    {
        StartTime = Time.time;
    }

    public void LanceCheckPanel()
    {
        CheckPanel.SetActive(true);
    }

    public void NotSure()
    {
        CheckPanel.SetActive(false);
    }

    public void LanceEffect()
    {
        StartCoroutine(BloqueEffect());
    }

    public IEnumerator BloqueEffect()
    {
        VFXBlock.SetActive(true);
        Avatar.SetActive(false);
        Debug.Log("Je");
        ChatCanva.SetActive(false);
        Debug.Log("suis");

        for (int i = 0; i < PropsAvatar.Length; i++)
        {
            PropsAvatar[i].SetActive(false);
            Debug.Log("près");
        }

        for (int i = 0; i < PropsDistorion.Length; i++)
        {
            PropsDistorion[i].SetActive(false);
            Debug.Log("derrière");
        }
        
        ActiveTime = Time.time - StartTime;
        Self.SetActive(false);
        CheckPanel.SetActive(false);
        AudioAvatar.SetActive(false);
        AudioHUB.SetActive(true);
        username.SetActive(false);

        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < PropsHere.Length; i++)
        {
            PropsHere[i].SetActive(true);
            Debug.Log("toi");
        }

        Blocked = true;
        Debug.Log("DestroyAllCubes is called");
        dialogueManager.DestroyAllCubes();
        Debug.Log("DestroyAllCubes was called !!!!");

        yield return new WaitForSeconds(1f);
        {
            HUBTel.TeleportToHUB();
        }
    }
}
