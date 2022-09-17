using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newEntityData", menuName ="Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxHealth = 200f;

    public float damageHopSpeed = 4f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 1f;
    public float groundCheckRadius = 0.3f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    public GameObject hitParticle;

    public float closeRangeActionDistance = 0.5f;

    public LayerMask whatIsGround;

    public AudioClip[] enemyClips;
    public GameObject dropOnDeath;
}
