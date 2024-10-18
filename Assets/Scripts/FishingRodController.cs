using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FishingRodController : MonoBehaviour
{
    [Header("Reel")]
    [SerializeField] GameObject reel;

    private ReelHandle reelhandle;

    private void Start()
    {
        reelhandle = reel.transform.GetChild(0).GetComponent<ReelHandle>();
    }

    public void OnSelectEnter(SelectEnterEventArgs args) => reelhandle.enabled = true;
    public void OnSelectExit(SelectExitEventArgs args) => reelhandle.enabled = false;
}
