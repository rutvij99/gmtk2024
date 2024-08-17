using UnityEngine;
using UnityEngine.UI;

public class ShadowDetectionAutomatic : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public Transform directionalLight; // Assign the directional light in the inspector
    public Image healthBar; // Assign the health bar UI Slider in the Inspector
    public float maxHealth = 100f; // Maximum health
    public float healthDecayRate = 50f; // Health decay rate when not in shadow
    public GameObject deathEffect; // Assign the death particle effect prefab in the Inspector
    public Transform respawnPoint; // Assign the respawn point in the Inspector
    public LayerMask safeAreaLayerMask; // Layer mask for safe areas
    public float jumpForce = 5f; // Force applied for jumping
    public float gravity = -9.81f; // Gravity force
    public float jumpHeight = 2f; // Height of the jump
    public LayerMask groundLayer; // Layer to define what is considered ground for jumping

    private float _currentHealth;
    private CharacterController _controller;
    private Vector3 _targetSafeAreaPosition;
    private bool _isMoving;
    private Transform _currentSafeArea;
    private Vector3 _velocity;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _currentHealth = maxHealth;
        UpdateHealthBar();
        _isMoving = false;
        _currentSafeArea = null;
    }

    void Update()
    {
        // Check if the player is grounded
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Small downward force to keep player grounded
        }

        // If the player is in shadow, find the next valid safe area and move towards it
        if (IsInShadow() || IsAboveSafeArea())
        {
            if (!_isMoving)
            {
                _targetSafeAreaPosition = FindNextSafeAreaInShadow();
                if (_targetSafeAreaPosition != Vector3.zero) // Ensure a valid target was found
                {
                    _isMoving = true;
                }
            }

            if (_isMoving)
            {
                MoveTowardsTarget(_targetSafeAreaPosition);
            }
        }
        else
        {
            _isMoving = false;
        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // Check if health is depleted
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    bool IsInShadow()
    {
        Ray ray = new Ray(transform.position, -directionalLight.forward);
        RaycastHit hit;

        // Raycast in the direction opposite to the light source
        if (Physics.Raycast(ray, out hit))
        {
            // If the ray hits an object that is not the ground and tagged as "Obstacle", we are in shadow
            return hit.collider != null && hit.collider.tag == "Obstacle";
        }

        // If nothing is hit, we are not in shadow
        return false;
    }
    
    bool IsAboveSafeArea()
    {
        Ray ray = new Ray(transform.position + new Vector3(0,0.5f,0), Vector3.down);
        RaycastHit hit;
    
        // Raycast downward to check if the player is above a safe area
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, safeAreaLayerMask))
        {
            return hit.collider != null;
        }
    
        return false;
    }

    Vector3 FindNextSafeAreaInShadow()
    {
        Collider[] safeAreas = Physics.OverlapSphere(transform.position, Mathf.Infinity, safeAreaLayerMask);
        Vector3 closestSafeAreaPosition = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (Collider safeArea in safeAreas)
        {
            // Skip the current safe area the player is already on
            if (_currentSafeArea != null && safeArea.transform == _currentSafeArea)
            {
                continue;
            }

            // Check if the safe area is in the path of the shadow
            if (IsInShadowPath(safeArea.transform.position))
            {
                float distance = Vector3.Distance(transform.position, safeArea.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSafeAreaPosition = safeArea.transform.position;
                }
            }
        }

        return closestSafeAreaPosition;
    }

    bool IsInShadowPath(Vector3 position)
    {
        Vector3 directionToSafeArea = position - transform.position;
        Ray ray = new Ray(transform.position, directionToSafeArea.normalized);
        RaycastHit hit;

        // Check if the ray hits the safe area while passing through the shadow
        if (Physics.Raycast(ray, out hit, directionToSafeArea.magnitude, safeAreaLayerMask))
        {
            if (hit.collider != null && hit.collider.transform.position == position)
            {
                return true;
            }
        }

        return false;
    }

    void MoveTowardsTarget(Vector3 targetPosition)
    {
        // Calculate the direction to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // If the target position is elevated and the player is close, trigger jump
        if (targetPosition.y > transform.position.y + 0.1f && Vector3.Distance(transform.position, targetPosition) < 1.0f)
        {
            if (_isGrounded)
            {
                Jump();
            }
        }
        else
        {
            // Move horizontally towards the target
            _controller.Move(direction * speed * Time.deltaTime);
        }

        // Stop moving if the shadow is no longer below the player
        if (!IsInShadow())
        {
            _isMoving = false;
        }

        // If the player reaches the target position's center, update the current safe area and find the next one
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            _isMoving = false;
            _currentSafeArea = FindSafeAreaAtPosition(targetPosition);

            // Find the next safe area after reaching the center of the current one
            _targetSafeAreaPosition = FindNextSafeAreaInShadow();
            if (_targetSafeAreaPosition != Vector3.zero)
            {
                _isMoving = true; // Start moving towards the next safe area
            }
        }
    }

    void Jump()
    {
        // Jump by setting the velocity directly for the CharacterController
        _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    Transform FindSafeAreaAtPosition(Vector3 position)
    {
        Collider[] safeAreas = Physics.OverlapSphere(position, 0.1f, safeAreaLayerMask);
        foreach (Collider safeArea in safeAreas)
        {
            return safeArea.transform;
        }
        return null;
    }

    void NotInShadow()
    {
        Debug.Log("Not in shadow");

        // Decrease health over time when not in shadow
        _currentHealth -= healthDecayRate * Time.deltaTime;
        UpdateHealthBar();
    }

    void Die()
    {
        // Play death particle effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Respawn player at the start point
        transform.position = respawnPoint.position + Vector3.up;

        // Reset health
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = _currentHealth / maxHealth;
        }
    }
}
