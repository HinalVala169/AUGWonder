using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPatrol : MonoBehaviour
{
    public static event Action<int> OnInfoPointReached;

    public Transform[] patrolPoints;
    public List<int> broadcastPoints;
    private int currentBroadcastIndex;
    public int broadcastIndex;
    private bool isPatrolling;
    public int targetPoint;
    public float speed;
    public float rotationSpeed = 5f; // Speed of rotation
    public float waitTime = 2f; // Time to wait at each patrol point
    public List<float> waitTimeList;

    private bool isWaiting = false;
    private Coroutine patrolCoroutine;

    [SerializeField] private bool isMainCharacter; //so that only one broadcast is going
    private Animator animator; // Reference to the Animator

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);  // Stop the coroutine when the object is disabled
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
        // if (patrolCoroutine == null)  // Make sure it doesn't start multiple times
        // {
           
        // }
        isPatrolling = true; 
        patrolCoroutine = StartCoroutine(Patrol());
        AudioManager.Instance.PlayNextVoiceOverClip();
    }

    private IEnumerator Patrol()
    {
        while (isPatrolling)
        {   //Debug.Log("Walk");

           // Debug.Log("----=== :");
            // Move towards the target patrol point
            while (transform.position != patrolPoints[targetPoint].position)
            {
                // Move the character
                transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

                // Calculate the direction to the target
                Vector3 direction = patrolPoints[targetPoint].position - transform.position;

                // Calculate the rotation step
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                animator.SetBool("HandRaiseDone", false);
                // Set animator to walking
                animator.SetBool("Walk", true);

                yield return null; // Wait for the next frame
            }

            // Stop walking animation when reached the point
           animator.SetBool("Walk", false);
            //animator.SetBool("HandRaiseDone", true);
            if (broadcastPoints.Contains(targetPoint))
            {
                broadcastIndex = broadcastPoints.IndexOf(targetPoint);
                if (isMainCharacter) // Only the main character broadcasts
                {
                  //  Debug.Log("----> :" + broadcastIndex);
                     animator.SetBool("Walk", false);
                     //animator.Stop("Walk");
                    //animator.SetBool("HandRaiseDone", true);
                    PlayHandAnim();

                    OnInfoPointReached?.Invoke(broadcastIndex); // Broadcast the index
                }

                // Wait at the patrol point for a specified time before moving to the next
                yield return new WaitForSeconds(waitTimeList[broadcastIndex]);
            }

            // Increase the target point index
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
            targetPoint = 0; // Loop back to the start
        }
    }

   public void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine); // Stop the patrol coroutine
            patrolCoroutine = null; // Reset the coroutine reference
        }
       // animator.enabled = false;
        // Reset the animator or other states as needed
        animator.SetBool("Walk", false);
        animator.SetBool("HandRaiseDone", false);
        animator.SetBool("PlayHandAnim", false);
        //animator.SetTrigger("isDefault");
        Debug.Log("Patrol stopped.");
        InfoManager.Instance.SetDefaultText();
        animator.Play("Default");
       // Debug.Log("Default");
        isPatrolling = false;
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // Check if the other object has the tag "Pulse"
    //     if (other.CompareTag("pulse"))
    //     {
    //         Debug.Log("Character triggered with Pulse!");
    //         //other.gameobject.SetActive(false);
    //         //other.gameObject.SetActive(false);
    //     }
    // }

    
}
