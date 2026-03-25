using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackEffectPrefab;
    public Transform firePoint;
    
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Instantiate(attackEffectPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
    }
}
