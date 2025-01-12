using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
     


public class IndiaGateTrigger : MonoBehaviour
{
    public static event Action<bool,bool> OnIndiaGateTrigger;
    

    [SerializeField] private bool cameraToCenterTrigger;
        [SerializeField] private bool cameraRot;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnIndiaGateTrigger?.Invoke(cameraToCenterTrigger , cameraRot);
        }
    }
}
