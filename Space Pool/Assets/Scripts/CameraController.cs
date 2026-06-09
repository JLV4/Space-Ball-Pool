using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls a camera orbiting around a target object with smooth motion, adjustable rotation, and zoom.
/// Includes an optional gimbal lock for vertical rotation limiting.
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 baseOffset = new Vector3(0f, 50f, -200f);

    public float rotationSpeed = 100f;
    public float smoothSpeed = 10f;

    public float minPitch = -89.9f;
    public float maxPitch = 89.9f;

    public float minZoom = 50f;
    public float maxZoom = 500f;
    public float zoomSpeed = 500f;

    public bool useRotationClamp = true;

    private float currentYaw = 0f;
    private float currentPitch = 20f;
    private float currentZoom;

    /// <summary>
    /// Initializes the controller and sets the initial zoom level
    /// based on the magnitude of the base offset.
    /// </summary>
    void Start()
    {
        currentZoom = baseOffset.magnitude;
    }

    /// <summary>
    /// Updates the camera each frame after all other objects have moved.
    /// Handles input, rotation, zoom, and applies final transformations.
    /// </summary>
    void LateUpdate()
    {
        if (target == null) return;

        HandleInput(out float horizontalInput, out float verticalInput, out float zoomInput);
        UpdateRotation(horizontalInput, verticalInput);
        UpdateZoom(zoomInput);
        ApplyCameraTransform();
    }

    /// <summary>
    /// Handles keyboard and mouse input for rotation and zoom.
    /// </summary>
    /// <param name="horizontalInput">Output for horizontal rotation input.</param>
    /// <param name="verticalInput">Output for vertical rotation input.</param>
    /// <param name="zoomInput">Output for zoom input.</param>
    private void HandleInput(out float horizontalInput, out float verticalInput, out float zoomInput)
    {
        horizontalInput = 0f;
        verticalInput = 0f;
        zoomInput = 0f;

        // Keyboard rotation input
        if (Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1f;
        else if (Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1f;

        if (Keyboard.current.upArrowKey.isPressed) verticalInput = 1f;
        else if (Keyboard.current.downArrowKey.isPressed) verticalInput = -1f;

        // Mouse rotation input
        if (Mouse.current.rightButton.isPressed)
        {
            horizontalInput += Mouse.current.delta.x.ReadValue() * 0.1f;
            verticalInput += Mouse.current.delta.y.ReadValue() * 0.1f;
        }

        // Mouse scroll zoom input
        if (Mouse.current.scroll.y.ReadValue() != 0f)
            zoomInput = -Mouse.current.scroll.y.ReadValue();
    }

    /// <summary>
    /// Updates the camera�s yaw and pitch rotation based on user input.
    /// Applies clamping if gimbal lock is enabled.
    /// </summary>
    /// <param name="horizontalInput">Horizontal input value.</param>
    /// <param name="verticalInput">Vertical input value.</param>
    private void UpdateRotation(float horizontalInput, float verticalInput)
    {
        currentYaw += horizontalInput * rotationSpeed * Time.deltaTime;
        currentPitch -= verticalInput * rotationSpeed * Time.deltaTime;

        if (useRotationClamp)
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
    }

    /// <summary>
    /// Updates the camera zoom distance based on scroll input.
    /// </summary>
    /// <param name="zoomInput">Zoom input value from mouse scroll or other control.</param>
    private void UpdateZoom(float zoomInput)
    {
        currentZoom += zoomInput * zoomSpeed * Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    /// <summary>
    /// Calculates the offset from the target based on the current zoom level.
    /// </summary>
    /// <returns>Returns a Vector3 offset for the camera�s position.</returns>
    private Vector3 CalculateOffset()
    {
        return new Vector3(0f, 0f, -currentZoom);
    }

    /// <summary>
    /// Applies the camera�s final position and rotation using smooth interpolation.
    /// </summary>
    private void ApplyCameraTransform()
    {
        Vector3 offset = CalculateOffset();
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
}
