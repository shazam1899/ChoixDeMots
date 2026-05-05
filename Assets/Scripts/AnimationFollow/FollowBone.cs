using UnityEngine;

public class FollowBone : MonoBehaviour
{
    public Transform bone;
    public Vector3 positionoffset;
    public Vector3 rotationOffset;

    void LateUpdate()
    {
        if (bone != null)
        {
            transform.position = bone.position + positionoffset;
            transform.rotation = bone.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
