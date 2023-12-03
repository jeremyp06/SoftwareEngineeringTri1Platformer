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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimeCounter = jumpTime;
        _explodable = GetComponent<Explodable>();

        movementOption = getMovementOption(playerData.level);
    }

    private void getMovementOption(int level){
        if (level == 1){
            return levelOneMovement;
        } else if (level == 2){
            return levelTwoMovement;
        } else if (level == 3){
            return levelThreeMovement;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        movementOption = getMovementOption(playerData.level);
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
        }
    }

    private void Movement(){
        Vector2 bottomLeftCorner = new Vector2(transform.position.x - GetComponent<BoxCollider2D>().bounds.extents.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);
        Vector2 bottomRightCorner = new Vector2(transform.position.x + GetComponent<BoxCollider2D>().bounds.extents.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);

        bool isLeftGrounded = Physics2D.OverlapCircle(bottomLeftCorner, checkRadius, groundLayer);
        bool isRightGrounded = Physics2D.OverlapCircle(bottomRightCorner, checkRadius, groundLayer);

        isGrounded = isLeftGrounded || isRightGrounded;

        if (isGrounded)
        {
            jumpTimeCounter = jumpTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        } else if (isJumping) {
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

        if (movementOption.CanDash()){
            bool dashInput = Input.GetKeyDown(KeyCode.UpArrow);

            if (dashInput && canDash)
            {
                isDashing = true;
                canDash = false;
                trailRenderer.emitting = true;

                dashDirection = new Vector2(moveInput, 0f).normalized;
                
                if (moveInput == 0){
                    float characterDirection = isFacingRight ? 1f : -1f;
                    dashDirection = new Vector2(characterDirection, 0f).normalized;
                }

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
        StartCoroutine(TempDisableMovement());
    }

    public void Die()
    {
        _explodable.explode();
    }

    private IEnumerator TempDisableMovement(){
        movementDisabled = true;
        yield return new WaitForSeconds(3f);
        movementDisabled = false;
    }
}