using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public MovementOption movementOption;
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


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimeCounter = jumpTime;
    }

    private void FixedUpdate()
    {
        playerData.currentPosition = transform.position; // Update currentPosition constantly
    }

    private void Update()
    {

        Vector2 bottomLeftCorner = new Vector2(playerData.currentPosition.x - GetComponent<BoxCollider2D>().bounds.extents.x, playerData.currentPosition.y - GetComponent<BoxCollider2D>().bounds.extents.y);
        Vector2 bottomRightCorner = new Vector2(playerData.currentPosition.x + GetComponent<BoxCollider2D>().bounds.extents.x, playerData.currentPosition.y - GetComponent<BoxCollider2D>().bounds.extents.y);

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


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }


}