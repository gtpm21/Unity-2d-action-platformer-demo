using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSoundManager : MonoBehaviour
{
    private PlayerController2D PC;

    [SerializeField]
    AudioClip[] footstepClips;

    void Start()
    {
        PC = GetComponent<PlayerController2D>();
    }

    public void PlayFootstepSound()
    {
        if (PC.isMoving && PC.isGrounded)
        {
            int index = Random.Range(0, footstepClips.Length);
            AudioClip clip = footstepClips[index];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}
