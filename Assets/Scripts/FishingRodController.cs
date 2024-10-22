using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FishingRodController : MonoBehaviour
{
    public UnityEvent OnRodThrowed;

    [Header("Rod")]
    [SerializeField] bool isThrowed;
    [SerializeField] Transform endPoint;
    [SerializeField] float power;

    [Header("Reel")]
    [SerializeField] GameObject reel;
    private ReelHandle reelhandle;

    [Header("Fish")]
    [SerializeField] Fish[] fishPrefabs;
    [SerializeField] Transform SpawnPoint;
    public Fish currentFish;

    [Header("Sounds")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sfxs;
    public enum SfxType
    {
        Swing0, Swing1, Splash, FishSplash
    }

    private XRBaseControllerInteractor interactor;

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

    public void Throw()
    {
        int fishType = Random.Range(0, fishPrefabs.Length);

        audioSource.clip = sfxs[fishType % 2];
        audioSource.Play();

        splashCoroutine = StartCoroutine(SplashRoutine());

        currentFish = Instantiate(fishPrefabs[fishType], SpawnPoint.position, fishPrefabs[fishType].transform.rotation);
        currentFish.OnCaught += CatchFish;
        currentFish.OnBitten += BittenFeedback;
    }

    Coroutine splashCoroutine;
    IEnumerator SplashRoutine()
    {
        yield return new WaitForSeconds(0.8f);

        audioSource.clip = sfxs[(int)SfxType.Splash];
        audioSource.Play();
    }

    private void BittenFeedback()
    {
        audioSource.clip = sfxs[(int)SfxType.FishSplash];
        audioSource.Play();

        if (interactor != null)
            interactor.SendHapticImpulse(0.8f, 0.8f);
    }

    private void CatchFish()
    {
        currentFish.OnCaught -= CatchFish;
        currentFish.OnBitten -= BittenFeedback;
        currentFish.transform.position = endPoint.position;
        currentFish.GetComponent<Rigidbody>().velocity = -power * endPoint.right;
        currentFish.gameObject.SetActive(true);
        currentFish = null;

        if (interactor != null)
            interactor.SendHapticImpulse(0.5f, 0.5f);
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        interactor = (XRBaseControllerInteractor)args.interactorObject;
        reelhandle.enabled = true;
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        interactor = null;
        reelhandle.enabled = false;

        if(currentFish != null)
        {
            currentFish.OnCaught -= CatchFish;
            currentFish.OnBitten -= BittenFeedback;
        }
    }

    public void OnDeactivate(DeactivateEventArgs args) => IsThrowed = true;
}
