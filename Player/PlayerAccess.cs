using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component acts as an easy general access to all player related components.
/// </summary>
public class PlayerAccess : MonoBehaviour
{
    public PlayerHealth PlayerHealth { get; private set; } = null;
    public PlayerMovement PlayerMovement { get; private set; } = null;
    public PlayerAiming PlayerAiming { get; private set; } = null;
    public PlayerShooting PlayerShooting { get; private set; } = null;
    public PlayerAnimations PlayerAnimations { get; private set; } = null;

    void Awake()
    {
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAiming = GetComponent<PlayerAiming>();
        PlayerShooting = GetComponent<PlayerShooting>();
        PlayerAnimations = GetComponent<PlayerAnimations>();
    }
}
