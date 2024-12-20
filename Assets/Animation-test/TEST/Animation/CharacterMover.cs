using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed at which the character moves forward.
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Move the character forward.
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Play the walk animation.
        if (animator)
        {
            animator.SetBool("isWalking", true); // Replace "isWalking" with your animation parameter name.
        }
    }
}
