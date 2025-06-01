using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSoundHandler : MonoBehaviour
{
    [Header("DamageSound")]
    [SerializeField] private AudioClip damageClip;

    private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DamageSound()
    {
        StartCoroutine(Damage());
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.15f);
        audioSource.PlayOneShot(damageClip);
    }
}
