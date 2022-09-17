using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float
        inputTimer,
        attackRange,
        stunDamageAmount = 1f;
    [SerializeField]
    private float[] attackDamage = { 20f, 20f, 30f };
        
    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private LayerMask whatIsEnemies;
    [SerializeField]
    AudioClip[] audioclips;

    private bool gotInput;

    public bool isAttacking;
    public bool isBlocking;

    private int attackCount = 0;
    
    public Animator playerAnim;

    CinemachineImpulseSource impulse;

    private PlayerController2D PC;
    private PlayerStats PS;
    private AudioSource AS;
    private Rigidbody2D rb;

    private float lastInputTime = Mathf.NegativeInfinity;

    private AttackDetails attackDetails;

    private void Start()
    {
        PS = GetComponent<PlayerStats>();
        impulse = transform.GetComponent<CinemachineImpulseSource>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController2D>();
        AS = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (!PauseMenuScript.gameIsPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (combatEnabled)
                {
                    gotInput = true;
                    lastInputTime = Time.time;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                gotInput = true;
                Block();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isBlocking = false;
                playerAnim.SetBool("isBlocking", isBlocking);
            }
        }
    }

    private void Block()
    {
        if (!PC.isFalling && !PC.isRolling && !PC.isWallSliding && PC.isGrounded && !PC.isJumping)
        {
            isBlocking = true;
        }
    }

    private void CheckAttacks()
    {
        if (gotInput && !isBlocking && PC.isGrounded)
        {

            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                attackCount = 1;
                playerAnim.SetInteger("attackCount", attackCount);
                playerAnim.SetBool("isAttacking", isAttacking);
            }
            else if(isAttacking && attackCount == 2 && Time.time <= lastInputTime + inputTimer)
            {
                gotInput = false;
                isAttacking = true;
                playerAnim.SetBool("isAttacking", isAttacking);
            }
            else if(isAttacking && attackCount == 3 && Time.time <= lastInputTime + inputTimer)
            {
                gotInput = false;
                isAttacking = true;
                playerAnim.SetBool("isAttacking", isAttacking);     
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            ResetAttack();
        }
    }

    private void CheckAttackHitbox()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

        attackDetails.damageAmount = attackDamage[attackCount - 1];
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (Collider2D collider in enemiesToDamage)
        {
            if (collider.tag == "AliveEnemy")
            {
                collider.transform.SendMessageUpwards("TakeDamage", attackDetails);
                impulse.GenerateImpulse();
                AS.PlayOneShot(audioclips[1]);
            }
        }
    }

    private void FinishAttack1()
    {
        attackCount = 2;
        playerAnim.SetInteger("attackCount", attackCount);
    }
    private void FinishAttack2()
    {
        attackCount = 3;
        playerAnim.SetInteger("attackCount", attackCount);
    }
    private void FinishAttack3()
    {
        ResetAttack();
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (!PC.GetRollStatus() && !PS.isDead && !isBlocking)
        {
            int direction;

            PS.DecreaseHealth(attackDetails.damageAmount);
            AS.PlayOneShot(audioclips[2]);

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            PC.Knockback(direction);
            playerAnim.Play("Player_Hurt");
            ResetAttack();
        }
    }

    private void PlayWooshSound()
    {
        AS.PlayOneShot(audioclips[0]);
    }

    private void ResetAttack()
    {
        gotInput = false;
        isAttacking = false;
        playerAnim.SetInteger("attackCount", 0);
        playerAnim.SetBool("isAttacking", false);
        PC.EnableFlip();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
