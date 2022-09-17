using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Vector2 move;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D player_Collider;
    private AudioSource AS;

    private float 
        movementInputDirection,
        knockbackStartTime;

    [SerializeField]
    private float knockbackDuration;
    [SerializeField]
    private Vector2 knockbackSpeed;

    public float
        movementSpeed = 10f,
        jumpForce = 13f,
        wallSlidingSpeed = 2,
        airDragMultiplier = 0.99f,
        jumpTimerSet = 0.15f,
        rollSpeed = 3f,
        attackMoveSpeedMultiplier = 0f;
        
    protected bool
        isFacingRight = true,
        canFlip = true,
        knockback;
    
    public bool
        isMoving,
        isGrounded,
        isTouchingWall,
        isFalling,
        isRolling,
        isWallSliding,
        isAttacking,
        isBlocking,
        isJumping;

    [SerializeField]
    public AudioClip[] playerClips;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player_Collider = GetComponent<CapsuleCollider2D>();
        AS = GetComponent<AudioSource>();
        
    }

    private void FixedUpdate()
    {
        ApplyMovement();       
    }
 
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfWallSliding();
        CheckIfFalling();
        CheckIfAttacking();
        CheckKnockback();
        CheckIfBlocking();
    }

    private void CheckIfBlocking()
    {
        isBlocking = GetComponent<PlayerCombat>().isBlocking;
    }

    private void CheckIfAttacking()
    {
        isAttacking = GetComponent<PlayerCombat>().isAttacking;
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isWallsliding", isWallSliding);
        anim.SetBool("isBlocking", isBlocking);
    }

    public bool GetRollStatus()
    {
        return isRolling;
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0 && !isRolling)
        {
            isWallSliding = true;
            isJumping = false;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckKnockback()
    {
        if((Time.time >= knockbackStartTime + knockbackDuration) && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void CheckIfFalling()
    {
        if (!isTouchingWall && !isGrounded && rb.velocity.y <= 0)
        {
            isFalling = true;
            isRolling = false;
            isJumping = false;
        }
        else
        {
            isFalling = false;
        }
    }

    private void CheckInput()
    {
        if (!PauseMenuScript.gameIsPaused)
        {
            movementInputDirection = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking && !isBlocking && !isRolling)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C) && isGrounded && !isBlocking && !isAttacking && !isRolling)
            {
                Roll();
            }
        }
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && !isRolling && !isAttacking && !knockback)
        {
            rb.velocity = new Vector2(movementInputDirection * movementSpeed * airDragMultiplier, rb.velocity.y);
        }
        else if (isGrounded && !isWallSliding && !isRolling && isAttacking && !knockback)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (!knockback && !isBlocking && !isRolling)
        {
            rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
        }
        else if (!isRolling && isGrounded)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (isRolling && isFacingRight)
        {
            rb.velocity = new Vector2((movementSpeed + rollSpeed), rb.velocity.y);
        }
        else if (isRolling && !isFacingRight)
        {
            rb.velocity = new Vector2(-(movementSpeed + rollSpeed), rb.velocity.y);
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }


    }

    public void DisableFlip()
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0 && canFlip)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0 && canFlip)
        {
            Flip();
        }

        if (rb.velocity.x > 0.1 || rb.velocity.x < -0.1)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void Flip()
    {
        if (!knockback)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void Jump()
    {
        isJumping = true;
        anim.SetTrigger("isJumping");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        AS.PlayOneShot(playerClips[0]);
    }

    private void Roll()
    {
        isRolling = true;
        player_Collider.size = new Vector2(0.7f, 0.5f);
        player_Collider.offset = new Vector2(0, 0.5f);
        anim.SetTrigger("isRolling");
    }

    private void ExitRoll()
    {
        player_Collider.size = new Vector2(0.7f, 1.25f);
        player_Collider.offset = new Vector2(0, 0.7f);
        isRolling = false;
    }
}
