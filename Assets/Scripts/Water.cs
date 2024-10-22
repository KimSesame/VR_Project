using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Respawnable respawnable = other.gameObject.GetComponent<Respawnable>();

        audioSource.Play();
        if (respawnable != null)
        {
            respawnable.Respawn();
        }
    }
}
