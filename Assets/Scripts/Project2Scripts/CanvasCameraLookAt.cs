using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible to make the canvas always face towards the player
/// </summary>
public class CanvasCameraLookAt : MonoBehaviour
{
    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float followSpeed;

    /// <summary>
    /// Initializes the script variables
    /// Assigns the camera transform the canvas needs to face
    /// </summary>
    /// <param name="camTransform"></param>
    public void Initialize(Transform camTransform)
    {
        playerCameraTransform = camTransform;
    }

    private void Update()
    {
        LookAt();
    }

    /// <summary>
    /// Function to update the rotation of the canvas gameobject based on the camera movement
    /// So that it always faces towards the camera transform assigned
    /// </summary>
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
