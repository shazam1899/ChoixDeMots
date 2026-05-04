using UnityEngine;
using UnityEngine.UI;

public class ReactContentSize : MonoBehaviour
{
    public GameObject contentSizeFitter;
    private ContentSizeFitter csf;

    // Update is called once per frame
    void Update()
    {
        csf = contentSizeFitter.GetComponentInChildren<ContentSizeFitter>();
    }
}
