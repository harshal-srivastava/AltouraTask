using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasCameraLookAt : MonoBehaviour
{
    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float followSpeed;

    public void Initialize(Transform camTransform)
    {
        playerCameraTransform = camTransform;
    }

    private void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        if (playerCameraTransform == null)
        {
            return;
        }
        transform.LookAt(this.transform.position + playerCameraTransform.transform.rotation * Vector3.forward, playerCameraTransform.transform.rotation * Vector3.up);
        this.transform.Rotate(0, 180, 0);
    }
}
