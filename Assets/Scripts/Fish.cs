using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Action OnBitten;
    public Action OnCaught;

    public bool isBitten = false;
    public int type;

    [SerializeField] float hp;

    private void OnEnable()
    {
        biteCoroutine = StartCoroutine(BiteRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(biteCoroutine);
    }

    Coroutine biteCoroutine;
    IEnumerator BiteRoutine()
    {
        Debug.Log("Fish Strolling...");
        yield return new WaitForSeconds(UnityEngine.Random.Range(3, 10));
        Debug.Log("Fish Bites!");
        Bite();
    }

    private void Bite()
    {
        isBitten = true;
        OnBitten?.Invoke();
    }

    public void EatBait(float damage)
    { 
        hp -= damage;
        if (hp < 0)
            OnCaught?.Invoke();
    }
}
