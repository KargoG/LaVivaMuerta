using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is a general parent class for all guns in the game.
/// All guns need a component that is a subclass of this class.
/// </summary>
public abstract class Gun : MonoBehaviour
{
    [Header("Gun General")]
    [SerializeField] protected GameObject _bulletPre = null;
    [SerializeField] protected Transform _barrelExit = null;

    [Header("Gun Specific")]
    [SerializeField] protected float _timeBetweenShots = 0.2f;
    public float CurrentTimeBetweenShots { get; set; }

    [Header("Gun Events")]
    [SerializeField] protected UnityEvent _onShot = new UnityEvent();

    protected float _timeOfLastShot = 0;

    // Container to keep track and order all bullets in the scene
    protected Transform _bulletContainer = null;

    public bool IsShooting { get; private set; } = false;

    protected virtual void Awake()
    {
        CurrentTimeBetweenShots = _timeBetweenShots;
    }

    protected virtual void Start()
    {
        _bulletContainer = new GameObject("Bullet Container").transform;
    }

    /// <summary>
    /// This method should be called every frame the player tries to shoot
    /// </summary>
    /// <param name="positionToShoot">The position the player is aiming at/ that should be shot at</param>
    public void Shoot(Vector3 positionToShoot)
    {
        bool successfull = HandleShot(positionToShoot);
        IsShooting = true;

        if (successfull)
        {
            _onShot.Invoke();
            OnShoot();
        }
    }

    /// <summary>
    /// This method handles the specifics of a weapons shot. This includes cooldowns and any weapon specific behaviour like charging.
    /// This method gets called EVERY FRAME that the player holds the shot button (usually LMB)
    /// </summary>
    /// <param name="positionToShoot">The position the player is aiming at/ that should be shot at</param>
    /// <returns>Did a bullet or something comparable get shot this frame?</returns>
    protected abstract bool HandleShot(Vector3 positionToShoot);


    /// <summary>
    /// Override this method to define additional behaviour for the gun. F.E. knock back the player on Shot.
    /// This method ONLY gets called if a SHOT was SUCCESSFULL this frame.
    /// </summary>
    protected abstract void OnShoot();

    /// <summary>
    /// This method only gets called the frame the player releases the shooting button (usually LMB) and stops shooting.
    /// </summary>
    public void StopShooting()
    {
        IsShooting = false;
        OnStopShooting();
    }

    /// <summary>
    /// Override this method to define additional behaviour when the player stops shooting
    /// </summary>
    protected virtual void OnStopShooting() { }


    public void AddOnShotListener(UnityAction listener)
    {
        _onShot.AddListener(listener);
    }

    public void ResetFireRate()
    {
        CurrentTimeBetweenShots = _timeBetweenShots;
    }
}
