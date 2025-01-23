using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Find the main camera and get its transform
        cameraTransform = Camera.main.transform;

        transform.LookAt(cameraTransform);
        Debug.Log("Camera Rotetion Done");
    }

    
}
