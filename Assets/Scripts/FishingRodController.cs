using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FishingRodController : MonoBehaviour
{
    public UnityEvent OnRodThrowed;

    [Header("Reel")]
    [SerializeField] GameObject reel;

    [Header("Fish")]
    [SerializeField] Fish[] fishPrefabs;
    public Fish currentFish;

    [SerializeField] bool isThrowed;

    private ReelHandle reelhandle;

    public bool IsThrowed {
        get { return isThrowed; } 
        set
        { 
            isThrowed = value;
            if (isThrowed) OnRodThrowed?.Invoke();
        } 
    }

    private void Start()
    {
        reelhandle = reel.transform.GetChild(0).GetComponent<ReelHandle>();
    }

    public void SelectFish()
    {
        int fishType = Random.Range(0, fishPrefabs.Length);

        currentFish = Instantiate(fishPrefabs[fishType], transform);
        currentFish.OnCaught += CatchFish;
    }

    public void CatchFish()
    {
        currentFish.OnCaught -= CatchFish;
        currentFish.transform.position = transform.position;
        currentFish.gameObject.SetActive(true);
    }

    public void OnSelectEnter(SelectEnterEventArgs args) => reelhandle.enabled = true;
    public void OnSelectExit(SelectExitEventArgs args) => reelhandle.enabled = false;
    public void OnDeactivate(DeactivateEventArgs args) => IsThrowed = true;
}
