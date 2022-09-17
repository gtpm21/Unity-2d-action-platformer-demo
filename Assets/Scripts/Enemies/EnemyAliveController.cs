using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAliveController : MonoBehaviour
{
    private enum State
    {
        Idle,
        Moving,
        Knockback, 
        Dead
    }

    private State currentState;
    private FieldOfView fov;

    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        maxHitpoints,
        knockbackDuration,
        idleDuration,
        currentHitpoints,
        lastAttackTime,
        attackCooldown,
        attackPos,
        attackRadius,
        attackDamage,
        playerDetected;
    [SerializeField]
    private Transform
        groundCheck,
        wallCheck;
    [SerializeField]
    private LayerMask
        whatIsGround,
        whatIsPlayer;
    [SerializeField]
    private Vector2 
        knockbackSpeed;
    [SerializeField]
    private GameObject hitParticle;

    private float 
        knockbackStartTime,
        idleStartTime;
    private float[] attackDetails = new float[2];

    private int 
        facingDirection,
        damageDirection;

    private Vector2 movement;

    private bool
        groundDetected,
        wallDetected;

    public bool isDead = false;

    private Rigidbody2D enemyrb;
    private Animator enemyAnim;


    private void Start()
    {
        GameObject.Find("Player");
        enemyrb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        currentHitpoints = maxHitpoints;
        facingDirection = 1;
        currentState = State.Moving;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    //--IDLE STATE-----------------------------------------------------------------------------------------

    private void EnterIdleState()
    {
        //Debug.Log("Entered EnterIdleState");
        idleStartTime = Time.time;
        movement.Set(0, 0);
        enemyrb.velocity = movement;
        enemyAnim.SetBool("isIdle", true);

    }

    private void UpdateIdleState()
    {
        //Debug.Log("Entered UpdateIdleState");

        if (Time.time >= idleStartTime + idleDuration)
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitIdleState()
    {
        //Debug.Log("Entered ExitIdleState");
        Flip();
        enemyAnim.SetBool("isIdle", false);
    }

    //--WALKING STATE-----------------------------------------------------------------------------------

    private void EnterMovingState()
    {
        //Debug.Log("Entered EnterMovingState");
        enemyAnim.SetBool("isMoving", true);
    }

    private void UpdateMovingState()
    {
        //Debug.Log("Entered UpdateMovingState");
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if (!groundDetected || wallDetected)
        {
            SwitchState(State.Idle);
        }
        else 
        {
            movement.Set(movementSpeed * facingDirection, enemyrb.velocity.y);
            enemyrb.velocity = movement;
        }
    }

    private void ExitMovingState()
    {
        //Debug.Log("Entered ExitMovingState");
        enemyAnim.SetBool("isMoving", false);
    }

    //--KNOCKBACK STATE---------------------------------------------------------------------------------------

    private void EnterKnockbackState()
    {
        //Debug.Log("Entered EnterKnockbackState");
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        enemyrb.velocity = movement;
        enemyAnim.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState()
    {
        //Debug.Log("Entered UpdateKnockbackState");
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitKnockbackState()
    {
        //Debug.Log("Entered ExitKnockbackState");
        enemyAnim.SetBool("Knockback", false);
    }

    //--DEAD STATE--------------------------------------------------------------------------------------------------

    private void EnterDeadState()
    {
        //Spawn particles
        movement.Set(0, 0);
        enemyrb.velocity = movement;
        enemyAnim.SetBool("isDying", true);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    //--OTHER FUNCTIONS----------------------------------------------------------------------------------------------

    public void TakeDamage(float[] attackDetails)
    {
        currentHitpoints -= attackDetails[0];

        Instantiate(hitParticle, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if(attackDetails[1] > transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //Hit particle

        if(currentHitpoints > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else if(currentHitpoints <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Idle:
                ExitIdleState();
                break;
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Idle:
                EnterIdleState();
                break;
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }
    private void isDying()
    {
        isDead = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
