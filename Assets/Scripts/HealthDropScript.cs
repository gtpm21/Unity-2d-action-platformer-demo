using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropScript : MonoBehaviour
{
    PlayerStats PS;
    Rigidbody2D rb;
    AudioSource AS;
    SpriteRenderer SR;

    public float healthDropValue = 10f;
    [SerializeField]
    private float thrust = 10.0f;
    void Start()
    {
        PS = FindObjectOfType<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        AS = GetComponent<AudioSource>();
        SR = GetComponent<SpriteRenderer>();

        rb.AddForce(new Vector2(0 , 1) * thrust, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ItemPickUp")
        {
            if (PS.currentHealth < PS.maxHealth)
            { 
                if (PS.maxHealth - PS.currentHealth < healthDropValue)
                {
                    PS.currentHealth = PS.maxHealth;
                    AS.Play(0);
                    SR.enabled = false;
                    Destroy(gameObject, 1);
                }
                else
                {
                    PS.currentHealth += healthDropValue;
                    AS.Play(0);
                    SR.enabled = false;
                    Destroy(gameObject, 0.5f);
                }
                
            }
        }
    }
}
