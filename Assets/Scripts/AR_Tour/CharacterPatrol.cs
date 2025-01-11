   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPatrol : MonoBehaviour
{
    public static event Action<int> OnInfoPointReached;

    public Transform[] patrolPoints;
    public List<int> broadcastPoints;
    public int currentBroadcastIndex;
    public int broadcastIndex;
    private bool isPatrolling;
    public int targetPoint;
    public float speed;
    public float rotationSpeed = 5f; 
    public float waitTime = 2f; 
    public List<float> waitTimeList;
    private bool isWaiting = false;
    private Coroutine patrolCoroutine;
 [SerializeField] 
    private IndiaGateCameraTransition cameraPos;

    [SerializeField] private bool isMainCharacter; 
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine); 
            patrolCoroutine = null;
        }
    }


    public void ChangeSCR()
    {
         animator.SetBool("HandRaiseDone", false);
         InfoManager.Instance.CHangeSCR();
    }

    public void WalkStart()
    {
        AudioManager.Instance.currentClipIndex = 0;
        InfoManager.Instance.highLightCam.SetActive(false);
        isPatrolling = true; 
        patrolCoroutine = StartCoroutine(Patrol());
        AudioManager.Instance.PlayNextVoiceOverClip();
    }

    private IEnumerator Patrol()
    {
        while (isPatrolling)
        {
            // Move towards the target patrol point
            while (transform.position != patrolPoints[targetPoint].position)
            {
                // Move the character
                transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

                Vector3 direction = patrolPoints[targetPoint].position - transform.position;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                animator.SetBool("HandRaiseDone", false);
                animator.SetBool("Walk", true);
                yield return null;
            }
           animator.SetBool("Walk", false);
            if (broadcastPoints.Contains(targetPoint))
            {
                broadcastIndex = broadcastPoints.IndexOf(targetPoint);
                if (isMainCharacter) 
                {
                    animator.SetBool("Walk", false);
                     
                    if(!AudioManager.Instance.IsVoiceOverPlaying())
                    {
                       PlayHandAnim();
                       OnInfoPointReached?.Invoke(broadcastIndex); 
                    }
                }
                yield return new WaitForSeconds(waitTimeList[broadcastIndex]);
            }
            IncreaseTargetInt();
        }
    }

    public void  PlayHandAnim()
    {
        animator.SetBool("HandRaiseDone", true);
        animator.Play("HandRaise");
    } 
    private void IncreaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            StopPatrol();
           
        }
    }

   public void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine); 
            patrolCoroutine = null; 
        }
        animator.SetBool("Walk", false);
        animator.SetBool("HandRaiseDone", false);
        animator.SetBool("PlayHandAnim", false);
        Debug.Log("Patrol stopped.");
        animator.Play("Default");
        targetPoint = 0; 
        cameraPos.currentCameraPositionIndex = cameraPos.cameraPositionNo;
        isPatrolling = false;
        InfoManager.Instance.SetDefaultText();

    }

}
