using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private MovementOption movementOption;
    public MovementOption levelOneMovement;
    public MovementOption levelTwoMovement;
    public MovementOption levelThreeMovement;
    public PlayerData playerData;

    public float speed = 5f;
    public float jumpForce = 40f;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public float jumpTime = 0.35f;

    public LayerMask wallLayer;
    public float wallSlideSpeed = 3f;


    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumping;

    private bool isWallSliding;
    public Vector2 wallJumpPower = new Vector2(40f, 40f); // Adjust this for wall jump power

    private bool isFacingRight = true;

    public float dashVelocity = 40f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.5f;

    public TrailRenderer trailRenderer;

    private bool isDashing = false;
    private bool canDash = true;
    private Vector2 dashDirection;

    private bool movementDisabled = false;
    private Explodable _explodable;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimeCounter = jumpTime;
        _explodable = GetComponent<Explodable>();

        playerData.level = SceneManager.GetActiveScene().buildIndex;

        movementOption = getMovementOption(playerData.level);
        animator = GetComponent<Animator>();
    }

    private MovementOption getMovementOption(int level){
        if (level == 1){
            Debug.Log("Level 1 movement active");
            return levelOneMovement;
            
        } else if (level == 2){
            Debug.Log("Level 2 movement active");
            return levelTwoMovement;
            
        } else {
            Debug.Log("Level 3 movement active");
            return levelThreeMovement;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        movementOption = getMovementOption(playerData.level);
        Debug.Log (playerData.level);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            GetComponent<Rigidbody2D>().velocity = dashDirection * dashVelocity;
        }
    }

    private void Update()
    {

        if (!movementDisabled){
            Movement();
            UpdateTimer();
            animator.SetBool("isWalking", false);
            animator.SetBool("isJumping", false);
        } 

        playerData.level = SceneManager.GetActiveScene().buildIndex;

        if (isGrounded){
            if (Input.GetAxis("Horizontal") != 0 && isGrounded){
                animator.SetBool("isWalking", true);
                animator.SetBool("isJumping", false);
            } else {
                animator.SetBool("isWalking", false);
                animator.SetBool("isJumping", false);
            }
        } else {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJumping", true);
        }
    }

    private void UpdateTimer(){
        playerData.currentTime += Time.deltaTime;
    }

    private void Movement(){
        Vector2 bottomLeftCorner = new Vector2(transform.position.x - GetComponent<BoxCollider2D>().bounds.extents.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);
        Vector2 bottomRightCorner = new Vector2(transform.position.x + GetComponent<BoxCollider2D>().bounds.extents.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);

        bool isLeftGrounded = Physics2D.OverlapCircle(bottomLeftCorner, checkRadius, groundLayer);
        bool isRightGrounded = Physics2D.OverlapCircle(bottomRightCorner, checkRadius, groundLayer);

        isGrounded = isLeftGrounded || isRightGrounded;

        if (isGrounded)
        {
            // Resets the jump time counter when the character is on the ground
            jumpTimeCounter = jumpTime;
        }

        // Checks for a spacebar press and ensures the character is grounded to initiate a jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }

        // Checks for continuous spacebar press while the character is jumping
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            // Allows for variable jump height based on remaining jump time
            if (jumpTimeCounter > 0)
            {
                // Applies upward velocity for the jump and decreases the jump time counter
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                // Ends the jumping state if the maximum jump time is reached
                isJumping = false;
            }
        }
        // Handles stopping the jump if the jump key is released or the character is not jumping anymore
        else if (isJumping)
        {
            isJumping = false;
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        if (movementOption.CanWallJump()){

            bool wallTouchingRight = Physics2D.OverlapCircle(bottomRightCorner, checkRadius, wallLayer);
            bool wallTouchingLeft = Physics2D.OverlapCircle(bottomLeftCorner, checkRadius, wallLayer);

            isWallSliding = (wallTouchingRight || wallTouchingLeft) && !isGrounded;

            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }

            float wallJumpDirection = 1f;
            if (wallTouchingRight) {
                wallJumpDirection = -1f;
            }

            if (isWallSliding && !isJumping && Input.GetKeyDown(KeyCode.Space)){
                rb.velocity = new Vector2(wallJumpPower.x * wallJumpDirection, wallJumpPower.y);
                StartCoroutine(MoveAwayFromWall(0.1f, 10f, wallJumpDirection));
                jumpTimeCounter = jumpTime;
                isJumping = true;
            } 
        }

        // Checks if the player can perform a dash based on certain conditions
        if (movementOption.CanDash())
        {
            // Checks if the dash input (Up Arrow key) is pressed
            bool dashInput = Input.GetKeyDown(KeyCode.UpArrow);

            // Checks if the dash input is detected and the player is allowed to dash
            if (dashInput && canDash)
            {
                // Indicates that the player is currently dashing
                isDashing = true;

                // Prevents the player from dashing again until the current dash is complete
                canDash = false;

                // Activates the trail effect for visual indication of the dash
                trailRenderer.emitting = true;

                // Sets the dash direction based on the player's input
                dashDirection = new Vector2(moveInput, 0f).normalized;

                // Checks if the player is not providing any horizontal input for the dash
                if (moveInput == 0)
                {
                    // Determines the character's direction (left or right) for the dash
                    float characterDirection = isFacingRight ? 1f : -1f;
                    
                    // Sets the dash direction to move horizontally based on the character's facing direction
                    dashDirection = new Vector2(characterDirection, 0f).normalized;
                }

                // Initiates a coroutine to stop the dash after a delay
                StartCoroutine(StopDashingAfterDelay());
            }
        }
    }

    private IEnumerator MoveAwayFromWall(float duration, float speed, float direction)
    {
        float timer = 0f;
        
        while (timer < duration && !isGrounded)
        {
            // Move the player away from the wall
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StopDashingAfterDelay()
    {
        yield return new WaitForSeconds(dashTime);

        trailRenderer.emitting = false; // Disable trail renderer
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    public void Progress()
    {
        // Initiates a coroutine to temporarily disable movement
        StartCoroutine(TempDisableMovement());
    }

    public void Die()
    {
        // Calls the explode() method from the _explodable object
        _explodable.explode();
    }

    private IEnumerator TempDisableMovement()
    {
        // Temporarily disables movement
        movementDisabled = true;
        // Waits for 3 seconds before enabling movement again
        yield return new WaitForSeconds(3f);
        movementDisabled = false; // Enables movement after the wait time
    }
}