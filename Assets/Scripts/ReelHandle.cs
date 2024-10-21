using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReelHandle : XRBaseInteractable
{
    [SerializeField] FishingRodController fishingRodController;
    [SerializeField] Transform reelCenter;
    [SerializeField] float twistSensitivity = 1.5f;

    private IXRSelectInteractor interactor = null;
    private Vector3 initialDir;
    private float currentAngle = 0f;
    private float lastAngle = 0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(StartGrab);
        selectExited.AddListener(EndGrab);
    }

    protected override void OnDisable()
    {
        selectEntered.RemoveListener(StartGrab);
        selectExited.RemoveListener(EndGrab);
        base.OnDisable();
    }

    public void StartGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject;
        initialDir = (transform.position - reelCenter.position).normalized;

        currentAngle = Vector3.SignedAngle(reelCenter.forward, initialDir, reelCenter.up);
        lastAngle = currentAngle;
    }

    public void EndGrab(SelectExitEventArgs args)
    {
        interactor = null;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (interactor != null)
            {
                Rotate();
            }
        }
    }

    private void Rotate()
    {
        if (interactor == null) return;

        var interactorTransform = interactor.GetAttachTransform(this);
        Vector3 currentDir = (reelCenter.position - interactorTransform.position).normalized;
        float newAngle = Vector3.SignedAngle(initialDir, currentDir, reelCenter.up);
        float angleDelta = newAngle - lastAngle;

        // No angleDelta, no reel rotation
        if (Mathf.Abs(angleDelta) < 5f)
            return;

        // Update
        lastAngle = newAngle;
        currentAngle += angleDelta * twistSensitivity;
        initialDir = currentDir;

        // Rotate reel
        transform.RotateAround(reelCenter.position, reelCenter.up, currentAngle);

        // Bait fish
        Fish baitedFish = fishingRodController.currentFish;
        if (baitedFish != null)
        {
            baitedFish.EatBait(Mathf.Abs(angleDelta) * 0.01f);
        }
    }
}
