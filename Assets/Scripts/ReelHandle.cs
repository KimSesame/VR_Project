using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReelHandle : XRBaseInteractable
{
    [SerializeField] Transform reelCenter;
    [SerializeField] float twistSensitivity = 1.5f;

    private IXRSelectInteractor interactor = null;
    private Vector3 currentDir;
    private float currentAngle = 0f;

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
        currentDir = (transform.position - reelCenter.position).normalized;

        currentAngle = Vector3.SignedAngle(reelCenter.forward, currentDir, reelCenter.up);
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
        Vector3 dir = (reelCenter.position - interactorTransform.position).normalized;
        float angleDelta = Vector3.SignedAngle(currentDir, dir, reelCenter.up);

        // Rotate reel
        currentAngle += angleDelta * twistSensitivity;
        transform.RotateAround(reelCenter.position, reelCenter.up, currentAngle);

        // Update
        currentDir = dir;
    }
}
