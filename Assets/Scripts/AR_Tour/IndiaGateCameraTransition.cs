using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndiaGateCameraTransition : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private List<Vector3> mainCameraPositions;

    [SerializeField] private List<Quaternion> rotPositions;


   // [SerializeField]
    public int currentCameraPositionIndex = 0;
     public int currentcameraRotIndexNo;

    public int cameraPositionNo;
     public int cameraRotNo;

    private void OnEnable()
    {
        IndiaGateTrigger.OnIndiaGateTrigger += IndiaGateTrigger_OnIndiaGateTrigger;
    }

    private void OnDisable()
    {
        IndiaGateTrigger.OnIndiaGateTrigger -= IndiaGateTrigger_OnIndiaGateTrigger;
    }
     
     public void Start()
     {
            cameraPositionNo = currentCameraPositionIndex;
            cameraRotNo   = currentcameraRotIndexNo;
     }

    private void IndiaGateTrigger_OnIndiaGateTrigger(bool obj, bool rot)
    {

        if(rot)
        {
           
             currentcameraRotIndexNo = (currentcameraRotIndexNo + 1) % rotPositions.Count;
              mainCamera.transform.rotation  = rotPositions[currentcameraRotIndexNo];

        }
        if (obj)
        {
            // If true, move to the next position in the list
            currentCameraPositionIndex = (currentCameraPositionIndex + 1) % mainCameraPositions.Count;
        }
        else
        {
            // If false, move to the previous position in the list
            currentCameraPositionIndex = (currentCameraPositionIndex - 1 + mainCameraPositions.Count) % mainCameraPositions.Count;
        }

        // Set the camera position to the current index in the list
        mainCamera.transform.position = mainCameraPositions[currentCameraPositionIndex];
    }
}
