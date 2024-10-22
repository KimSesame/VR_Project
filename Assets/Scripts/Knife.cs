using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] GameObject[] sushiPrefabs;

    private AudioSource audioSource;
    private int fishLayer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        fishLayer = LayerMask.NameToLayer("Fish");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Fish to sushi
        if (other.gameObject.layer == fishLayer)
        {
            int fishType = other.gameObject.GetComponent<Fish>().type;

            other.enabled = false;

            audioSource.Play();
            Instantiate(sushiPrefabs[fishType], other.transform.position, Quaternion.identity);

            Destroy(other.gameObject);
        }
    }
}
