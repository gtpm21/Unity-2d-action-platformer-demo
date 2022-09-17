using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceManager : MonoBehaviour
{
    public static GameObject player;
    public static Vector2 playerPosition;
    public static CapsuleCollider2D playerCollider;
    
    private void Awake()
    {
        player = this.gameObject;
        playerPosition = player.transform.position;
        playerCollider = player.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        playerPosition = player.transform.position;
    }
}
