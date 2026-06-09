using UnityEngine;

public class LockCameraRotation : MonoBehaviour
{
    void LateUpdate()
    {
        Quaternion parentRotation = transform.parent.rotation;

        float yRotation = parentRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}