using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private GameObject Player;

    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Ground"))
        {
            Player.GetComponent<PlayerController2D>().isTouchingWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Ground"))
        {
            Player.GetComponent<PlayerController2D>().isTouchingWall = false;
        }
    }

}

