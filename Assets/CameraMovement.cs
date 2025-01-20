using UnityEngine;

public class DragMoveCamera : MonoBehaviour
{
    public float dragSpeed = 4f; // Speed of drag movement
    public float scrollSpeed = 2.0f; // Speed of scrolling
    public float baseMoveSpeed = 5.0f; // Base speed of WASD movement
    public float zoomSpeedMultiplier = 0.5f; // Multiplier for WASD speed based on height
    public float minHeight = 10.0f; // Minimum camera height
    public float maxHeight = 50.0f; // Maximum camera height
    public float minTiltAngle = 45.0f; // Tilt angle at minimum height
    public float maxTiltAngle = 90.0f; // Tilt angle at maximum height
    public float moveDuration = 1.0f; // Duration for smooth movement

    private Vector3 dragOrigin; // Initial position when the drag starts
    private bool isDragging = false;
    private bool isMovingToNode = false;
    private Vector3 targetPosition;
    private float moveStartTime;
    public GameObject UI;

    void Update()
    {
        HandleDragging();
        HandleScrolling();
        HandleWASDMovement();
        HandleRaycastHit();
        SmoothMoveToNode();
    }

    void HandleDragging()
    {
        if (UI.activeSelf && !isMovingToNode) return; // Disable dragging if UI is active and not moving to node

        // Detect mouse down
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            dragOrigin = Input.mousePosition;
        }

        // Detect mouse up
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        // Handle dragging
        if (isDragging)
        {
            Vector3 difference = Input.mousePosition - dragOrigin;

            // Move camera by adjusting its position
            Vector3 move = new Vector3(-difference.x, 0, -difference.y) * dragSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

            // Update drag origin for continuous dragging
            dragOrigin = Input.mousePosition;
        }
    }

    void HandleScrolling()
    {
        if (UI.activeSelf && !isMovingToNode) return; // Disable scrolling if UI is active and not moving to node

        // Get scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            // Calculate new height
            float newHeight = Mathf.Clamp(transform.position.y - scrollInput * scrollSpeed, minHeight, maxHeight);

            // Update camera position
            Vector3 position = transform.position;
            position.y = newHeight;
            transform.position = position;

            // Adjust tilt angle based on height
            float t = Mathf.InverseLerp(minHeight, maxHeight, newHeight);
            float tiltAngle = Mathf.Lerp(minTiltAngle, maxTiltAngle, t);

            Vector3 rotation = transform.eulerAngles;
            rotation.x = tiltAngle;
            transform.eulerAngles = rotation;
        }
    }

    void HandleWASDMovement()
    {
        if (UI.activeSelf && !isMovingToNode) return; // Disable WASD movement if UI is active and not moving to node

        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Calculate movement direction
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Calculate speed based on height
        float heightFactor = (transform.position.y - minHeight) * zoomSpeedMultiplier;
        float moveSpeed = baseMoveSpeed + heightFactor;

        // Move camera
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleRaycastHit()
    {

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Node"))
                {
                    targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z - 7);
                    moveStartTime = Time.time;
                    isMovingToNode = true;
                }
            }
        }
    }

    void SmoothMoveToNode()
    {
        if (isMovingToNode)
        {
            float t = (Time.time - moveStartTime) / moveDuration;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            if (t >= 1.0f)
            {
                isMovingToNode = false;
            }
        }
    }
}
