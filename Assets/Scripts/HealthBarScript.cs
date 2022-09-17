using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth;
    private PlayerStats PS;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        PS = FindObjectOfType<PlayerStats>();
        maxHealth = PS.maxHealth;
    }

    private void Update()
    {
        currentHealth = PS.currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
