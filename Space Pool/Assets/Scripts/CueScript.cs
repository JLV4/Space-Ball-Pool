using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CueScript : MonoBehaviour
{
    [Header("Settings")]
    public GameSettings gameSettings;

    [Header("UI")]
    public Slider powerSlider;
    public Gradient powerGradient;
    public Image fillImage;

    [Header("Audio")]
    public AudioClip launchSFX;
    public float launchVolume = 1f;
    private Vector2 launchPitchRange = new Vector2(0.98f, 1.02f);

    private Rigidbody rb;
    private LineRenderer aimLine;
    private Camera mainCamera;
    private Renderer ballRenderer;
    private AudioSource audioSource;

    private bool isAiming = false;
    private bool canShoot = false;
    private bool wasMoving = false;
    private bool isRedTurn = true;
    private Vector2 dragStartScreenPos;

    private const float movementThreshold = 1f;

    /// <summary>
    /// Initializes component references and sets up the aim line and turn color.
    /// </summary>
    private void Start()
    {
        //new check added for settings customization
        rb = GetComponent<Rigidbody>();
        aimLine = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        ballRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        //SetupAimLine();
        SetTurnColor();

        if (powerSlider != null)
        {
            powerSlider.value = 0;
            powerSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the ball state each frame, managing turns, aiming, and visual feedback.
    /// </summary>
    private void Update()
    {
        if(gameSettings.gameOver) return;
        HandleTurnChange();
        canShoot = !IsBallMoving() && !isAiming;

        UpdateBallAppearance();
        HandleInput();

        wasMoving = IsBallMoving();
    }

    /// <summary>
    /// Configures the initial properties of the aim line renderer.
    /// </summary>
    private void SetupAimLine()
    {
        aimLine.positionCount = 2;
        aimLine.enabled = false;
    }

    /// <summary>
    /// Checks whether the ball is currently moving above the threshold velocity.
    /// </summary>
    /// <returns>True if the ball is moving, false otherwise.</returns>
    private bool IsBallMoving()
    {
        return rb.linearVelocity.magnitude >= movementThreshold;
    }

    /// <summary>
    /// Switches turns when the ball stops moving and updates its color.
    /// </summary>
    private void HandleTurnChange()
    {
        if (!IsBallMoving() && wasMoving)
        {
            isRedTurn = !isRedTurn;
            SetTurnColor();
        }
    }

    /// <summary>
    /// Updates the ball color based on whose turn it is.
    /// </summary>
    private void SetTurnColor()
    {
        ballRenderer.material.color = isRedTurn ? Color.red : Color.blue;
    }

    /// <summary>
    /// Updates the appearance of the ball based on whether it is moving.
    /// </summary>
    private void UpdateBallAppearance()
    {
        Color currentColor = isRedTurn ? Color.red : Color.blue;
        currentColor.a = IsBallMoving() ? GameSettings.movingAlpha : 1f;
        ballRenderer.material.color = currentColor;
    }

    /// <summary>
    /// Handles mouse input for aiming and shooting mechanics.
    /// </summary>
    private void HandleInput()
    {
        if (Mouse.current == null) return;

        if (canShoot && Mouse.current.leftButton.wasPressedThisFrame)
            BeginAiming();

        if (isAiming && Mouse.current.leftButton.isPressed)
            UpdateAiming();

        if (isAiming && Mouse.current.leftButton.wasReleasedThisFrame)
            LaunchBall();
    }

    /// <summary>
    /// Begins the aiming process when the player starts dragging.
    /// </summary>
    private void BeginAiming()
    {
        isAiming = true;
        aimLine.enabled = true;
        dragStartScreenPos = Mouse.current.position.ReadValue();
        if (powerSlider != null)
            powerSlider.gameObject.SetActive(true);
    }

    /// <summary>
    /// Updates the aim line position and color during the aiming process.
    /// </summary>
    private void UpdateAiming()
    {
        Vector2 dragDelta = GetClampedDragDelta();

        Vector3 forwardDir = GetForwardDirection();
        Vector3 aimEnd = transform.position + forwardDir * dragDelta.magnitude * 0.05f;

        if (powerSlider != null)
        {
            float powerRatio = dragDelta.magnitude / GameSettings.maxDragDistance;

            powerSlider.value = powerRatio * 100;

            if (fillImage != null)
            {
                fillImage.color = powerGradient.Evaluate(powerRatio);
            }
        }
    }

    /// <summary>
    /// Launches the ball by applying force in the forward direction based on drag distance.
    /// </summary>
    private void LaunchBall()
    {
        Vector3 forwardDir = GetForwardDirection();

        float dragMagnitude = Mathf.Min(GetDragDistance(), GameSettings.maxDragDistance);

        Vector3 launchForce = forwardDir * dragMagnitude * gameSettings.maxShotPower;

        rb.AddForce(launchForce, ForceMode.Impulse);
        
        if (launchSFX != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(launchPitchRange.x, launchPitchRange.y);
            audioSource.PlayOneShot(launchSFX, launchVolume);
        }

        EndAiming();
    }

    /// <summary>
    /// Ends the aiming process and disables the aim line.
    /// </summary>
    private void EndAiming()
    {
        isAiming = false;
        aimLine.enabled = false;
        if (powerSlider != null)
        {
            powerSlider.value = 0;
            powerSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Returns the normalized forward direction from the main camera to the ball.
    /// </summary>
    /// <returns>A normalized <see cref="Vector3"/> representing the forward direction.</returns>
    private Vector3 GetForwardDirection()
    {
        return (transform.position - mainCamera.transform.position).normalized;
    }

    /// <summary>
    /// Calculates and returns the clamped drag delta vector between the current mouse position and the drag start position.
    /// </summary>
    /// <returns>A <see cref="Vector2"/> representing the clamped drag delta in screen space.</returns>
    private Vector2 GetClampedDragDelta()
    {
        Vector2 dragCurrent = Mouse.current.position.ReadValue();
        Vector2 dragDelta = dragCurrent - dragStartScreenPos;

        if (dragDelta.magnitude > GameSettings.maxDragDistance)
            dragDelta = dragDelta.normalized * GameSettings.maxDragDistance;

        return dragDelta;
    }

    /// <summary>
    /// Calculates and returns the raw drag distance between the current and initial mouse positions.
    /// </summary>
    /// <returns>A float representing the drag distance in screen space.</returns>
    private float GetDragDistance()
    {
        return (Mouse.current.position.ReadValue() - dragStartScreenPos).magnitude;
    }
}
