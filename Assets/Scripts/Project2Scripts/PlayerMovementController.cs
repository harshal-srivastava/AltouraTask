using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for moving the player around the level in the first person mode
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float mouseSensitivity = 2.0f;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private float maxLookAngle = 80f;

    private float verticalLookRotation = 0;
    private Rigidbody playerRigidBody;

    [SerializeField]
    private Image teleportationEffectImage;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Look();
    }

    /// <summary>
    /// Function to move the player around based on the inputs, whether from keyboard or any other source
    /// </summary>
    private void Move()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveDirectionX + transform.forward * moveDirectionZ;
        playerRigidBody.velocity = new Vector3(moveDirection.x * speed, playerRigidBody.velocity.y, moveDirection.z * speed);
    }

    /// <summary>
    /// Function enabling the player to look around based on mouse inputs
    /// </summary>
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);

        playerCamera.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Function to show the teleportation visual effect
    /// </summary>
    public void ShowTeleportEffect()
    {
        StartCoroutine(ShowTeleportationEffectAnimation(0.3f));
    }

    /// <summary>
    /// Coroutine to show the fade in and out effect when user is being teleported
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator ShowTeleportationEffectAnimation(float time)
    {
        float rate = 1.0f / time;
        float t = 0.0f;
        float alpha = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            alpha = Mathf.Lerp(0, 1, t);
            teleportationEffectImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        t = 0;
        alpha = 1;
        teleportationEffectImage.color = new Color(0, 0, 0, 1);
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            alpha = Mathf.Lerp(1, 0, t);
            teleportationEffectImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

    }
}
