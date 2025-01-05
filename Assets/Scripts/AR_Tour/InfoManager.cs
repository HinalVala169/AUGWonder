using UnityEngine;
using TMPro; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class InfoManager : MonoBehaviour
{

    public static InfoManager Instance;

    public CharacterPatrol character;
    [Header("References")]
    [SerializeField] private List<GameObject> screens;
    [SerializeField] private List<Sprite> infoIMG;
    [SerializeField] private MonumentInfoSO infoSO; // Reference to the ScriptableObject
    [SerializeField] private TextMeshProUGUI headingText,heading2; // TMP UI Text for the heading
    [SerializeField] private TextMeshProUGUI detailsText; // TMP UI Text for the details

      [SerializeField] private Image infoImg;
      public GameObject goBTN;
       public GameObject highLightCam;
     [SerializeField] private int infoNo; 

     public IndiaGateCameraTransition cameraTransition;

    private void Start()
    {
        if(Instance ==  null)
        {
           Instance = this;
        }
       
        if (infoSO == null)
        {
            Debug.LogError("InfoSO is not assigned!");
            return;
        }

        if (headingText == null || detailsText == null)
        {
            Debug.LogError("Text components are not assigned!");
            return;
        }

        // Optionally, call this with an initial index, e.g., 0
      //  FetchAndDisplayInfoByIndex(infoNo);
    }

    public void CHangeSCR()
    {
        if(infoNo < infoSO.monumentInformation.monumentInformation.Length)
        {
            screens[0].SetActive(false);
            FetchAndDisplayInfoByIndex(infoNo);
            screens[1].SetActive(true);
            highLightCam.SetActive(true);
            AudioManager.Instance.PlayNextVoiceOverClip();
            StartCoroutine(WaitForVoiceOverToComplete());
        }
    }

    private IEnumerator WaitForVoiceOverToComplete()
    {
        while (AudioManager.Instance.IsVoiceOverPlaying())
        {
            yield return null; 
        }
        
        Debug.Log("Audio clip finished playing. Performing next action.");

        // Check if all clips have been played
        if (AudioManager.Instance.currentClipIndex >= AudioManager.Instance.voiceOverClips.Length)
        {
            Debug.Log("All voice-over clips have been completed.");
           // AudioManager.Instance.PlayVisitAgainClip();
        }
        else
        {
            float delay = 10f; // Set the desired delay in seconds
            yield return new WaitForSeconds(delay);
            Debug.Log("Delay complete. Performing next action.");
            BackTOPrevSCR();
        }
    }

    public void BackTOPrevSCR()
    {
         infoNo++;
        if(infoNo < infoSO.monumentInformation.monumentInformation.Length)
        {
              highLightCam.SetActive(false);
            FetchAndDisplayInfoByIndex(infoNo);
            screens[1].SetActive(false);
            screens[0].SetActive(true);
            //character.PlayHandAnim();
            StartCoroutine(PerformAfterDelay(1f)); 
        }
        // else{
        //   //  character.WalkStart();
        // }
       
    }

    private IEnumerator PerformAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(infoNo < infoSO.monumentInformation.monumentInformation.Length)
        {
            character.PlayHandAnim();
        }
        
    }
    private void FetchAndDisplayInfoByIndex(int index)
    {
        if (infoSO.monumentInformation != null && index >= 0 && index < infoSO.monumentInformation.monumentInformation.Length)
        {
            var infoEntry = infoSO.monumentInformation.monumentInformation[index];
            headingText.text = heading2.text  = infoEntry.title; // Set heading text
            infoImg.sprite = infoIMG[index];
            detailsText.text = string.Join("\n", infoEntry.details); // Set details text
        }
        else
        {
            headingText.text = "Invalid Index";
            detailsText.text = "No details available for the requested index.";
        }
    }

    public void SetDefaultText()
    {   
        //cameraTransition.currentCameraPositionIndex = -1;
        goBTN.SetActive(true);
         infoNo = 0;
        headingText.text = heading2.text = infoSO.monumentInformation.monumentInformation[0].title;
        detailsText.text =  infoSO.monumentInformation.monumentInformation[0].details[0];
        //infoImg = 
        screens[1].SetActive(false);
        screens[0].SetActive(true);
    }
    // public void UpdateDisplayByIndex(int newIndex)
    // {
    //     FetchAndDisplayInfoByIndex(newIndex);
    // }
}
