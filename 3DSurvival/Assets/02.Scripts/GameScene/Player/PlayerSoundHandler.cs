using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    [Header("StepSound")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepThreshold;
    [SerializeField] private float walkStepRate;
    [SerializeField] private float runStepRate;
    private float stepRate;
    private float footstepTime;

    [Header("JumpSound")]
    [SerializeField] private AudioClip jumpGruntClip;


    [Header("PunchSound")]
    [SerializeField] private AudioClip attackGruntClip;
    [SerializeField] private AudioClip punchClip;


    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    private PlayerController controller;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        StepSound();
    }

    void StepSound()
    {
        if (controller.MoveSpeed == controller.WalkSpeed)
        {
            stepRate = walkStepRate;
        }
        else
        {
            stepRate = runStepRate;
        }

        if (controller.IsGrounded)
        {
            if (_rigidbody.velocity.magnitude > footstepThreshold)
            {
                if (Time.time - footstepTime > stepRate)
                {
                    footstepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }
            }
        }
    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(jumpGruntClip);
    }

    public void AttackGruntSound()
    {
        audioSource.PlayOneShot(attackGruntClip);
    }

    public void PunchSound()
    {
        audioSource.PlayOneShot(punchClip, 0.5f);
    }
}
