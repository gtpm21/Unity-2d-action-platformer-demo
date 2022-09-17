using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    private AudioSource AS;
    private PlayerController2D PC;

    void Start()
    {
        player = gameObject.transform.parent.gameObject;
        anim = player.GetComponent<Animator>();
        AS = player.GetComponent<AudioSource>();
        PC = player.GetComponent<PlayerController2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isFalling", false);
            anim.SetBool("isWallsliding", false);
            anim.SetBool("isJumping", false);
            AS.PlayOneShot(PC.playerClips[1]);
            PC.isGrounded = true;
            //Debug.Log("OnGround");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            PC.isGrounded = false;
            //Debug.Log("OffGround");
        }
    }

}

