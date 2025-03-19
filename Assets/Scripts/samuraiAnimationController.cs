using UnityEngine;

public class SamuraiAnimationController : MonoBehaviour
{
    private Animator animator;

    // Boolean parameters for animation transitions
    public bool isShouting = false;
    public bool isSwinging = false;
    public bool isWalking = false;
    public bool isJumping = false;
    public bool isStanding = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set animation parameters based on conditions
        animator.SetBool("isShouting", isShouting);
        animator.SetBool("isSwinging", isSwinging);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isStanding", isStanding);

        // Logic to handle animation transitions

        if (isShouting)
        {
            // Transition to sittingAngry animation
            animator.Play("sittingAngry");
        }
        else if (isSwinging)
        {
            // Transition to swingJump animation
            animator.Play("swingJump");
        }
        else if (isWalking)
        {
            // Transition to walking animation
            animator.Play("walking");
        }
        else if (isJumping)
        {
            // Transition to jumpShot animation
            animator.Play("jumpShot");
        }
        else if (!isJumping && isStanding)
        {
            // Transition to HappyIdle animation when jumpShot is false and standing is true
            animator.Play("HappyIdle");
        }
        else if (!isWalking && isStanding)
        {
            // Transition to HappyIdle when walking is false and standing is true
            animator.Play("HappyIdle");
        }
        else if (!isShouting && !isSwinging && !isWalking && !isJumping && !isStanding)
        {
            // Fallback to sitting animation when no other actions are true
            animator.Play("sitting");
        }
    }
}
