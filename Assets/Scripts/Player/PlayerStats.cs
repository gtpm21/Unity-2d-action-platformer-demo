using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public float maxHealth;
    public float currentHealth;

    public bool isDead = false;

    private Animator anim;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0.0f && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("isDying", true);
        Destroy(gameObject, 4.0f);
        //GM.Respawn();
    }
}
