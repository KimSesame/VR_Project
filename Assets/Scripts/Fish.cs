using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Action OnCaught;

    [SerializeField] float hp;

    public void EatBait(float damage)
    { 
        hp -= damage;
        Debug.Log($"{hp}");
        if (hp < 0)
            OnCaught?.Invoke();
    }
}
