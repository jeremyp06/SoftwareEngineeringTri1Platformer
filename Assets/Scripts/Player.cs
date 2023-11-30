using UnityEngine;

public class Player : MonoBehaviour
{
    public MovementOption movementOption;
    public PlayerData playerData;

    public float speed = 5f;
    public float jumpForce = 40f;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public float jumpTime = 1f;

    public LayerMask wallLayer;
    public float wallSlideSpeed = 3f;


    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumping;

    private bool isWallSliding;
    private float wallJumpDirection;
    private float wallJumpTime = 0.5f; // Adjust this value for wall jump duration
    private float wallJumpCounter;
    public Vector2 wallJumpPower = new Vector2(10f, 10f); // Adjust this for wall jump power

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
            Debug.Log("Whee!!!");
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

            //check to see if you are on a wall.
            //if so, slide down the wall

            //also, if you are on this wall, then if space is pressed:
            //jump whee

            if (isFacingRight){
                WallSlide(bottomLeftCorner);
                WallJump(bottomLeftCorner);
            } else {
                WallSlide(bottomRightCorner);
                WallJump(bottomRightCorner);
            }
            
        }
    }

    private void WallSlide(Vector2 wallCheck)
    {
        isWallSliding = Physics2D.OverlapCircle(wallCheck, checkRadius, wallLayer) && !isGrounded;

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    private void WallJump(Vector2 wallCheck)
    {
        if (isWallSliding)
        {
            isWallSliding = false;
        }
        
        if (Physics2D.OverlapCircle(wallCheck, checkRadius, wallLayer) && !isGrounded)
        {
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;
        }

        if (wallJumpCounter > 0)
        {
            wallJumpCounter -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                rb.velocity = new Vector2(wallJumpPower.x * wallJumpDirection, wallJumpPower.y);
                wallJumpCounter = 0;
            }
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