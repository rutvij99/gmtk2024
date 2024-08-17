using UnityEngine;
using UnityEngine.UI;

public class ShadowDetection : MonoBehaviour
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
    public LayerMask groundLayer; // Layer to define what is considered ground for jumping
    
    
    private Vector3 _safeAreaRaycastOffset = new Vector3(0, 0.2f, 0);
    private float _currentHealth;
    private CapsuleCollider _collider;
    private Rigidbody _rb;
    private bool _isGrounded;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _collider = this.GetComponentInChildren<CapsuleCollider>();
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        // Move the player based on input
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, 0, moveY));

        // Check if the player is in the shadow
        if (!IsInShadow() && !IsAboveSafeArea())
        {
            NotInShadow();
        }
        else
        {
            InShadow();
        }

        // Check if health is depleted
        if (_currentHealth <= 0)
        {
            Die();
        }
        
        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    bool IsInShadow()
    {
        Ray ray = new Ray(transform.position, -directionalLight.forward);
        RaycastHit hit;

        // Visualize the ray in the scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        // Raycast in the direction opposite to the light source
        if (Physics.Raycast(ray, out hit))
        {
            // If the ray hits an object that is not the ground and tagged as "Obstacle", we are in shadow
            return hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.tag == "Obstacle";
        }

        // If nothing is hit, we are not in shadow
        return false;
    }
    
    // bool IsAboveSafeArea()
    // {
    //     Ray ray = new Ray(transform.position + _safeAreaRaycastOffset, Vector3.down);
    //     RaycastHit hit;
    //
    //     // Raycast downward to check if the player is above a safe area
    //     if (Physics.Raycast(ray, out hit, Mathf.Infinity, safeAreaLayerMask))
    //     {
    //         return hit.collider != null;
    //     }
    //
    //     return false;
    // }
    
    bool IsAboveSafeArea()
    {
        // Define the radius of the overlap sphere
        float sphereRadius = 0.5f; // Adjust the radius as needed

        // Perform an overlap sphere at the player's position + offset
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + _safeAreaRaycastOffset, sphereRadius, safeAreaLayerMask);

        // Check if any of the colliders in the sphere belong to the safe area
        foreach (Collider collider in hitColliders)
        {
            if (collider != null)
            {
                return true;
            }
        }

        return false;
    }
    
    bool IsGrounded()
    {
        // Check if the player is grounded using a raycast
        Ray ray = new Ray(transform.position+ _safeAreaRaycastOffset, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit hit, 1.1f, groundLayer);
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


    void NotInShadow()
    {
        Debug.Log("Not in shadow");

        // Decrease health over time when not in shadow
        _currentHealth -= healthDecayRate * Time.deltaTime;
        UpdateHealthBar();
    }

    void InShadow()
    {
        Debug.Log("In shadow");

        // Optionally, you could increase health over time when in shadow
        // currentHealth = Mathf.Min(currentHealth + (healthDecayRate * Time.deltaTime), maxHealth);
        // UpdateHealthBar();
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
