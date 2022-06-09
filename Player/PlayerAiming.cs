using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component handles everything related to aiming for the player.
/// The aim is based on the mouse position.
/// </summary>
public class PlayerAiming : MonoBehaviour
{
    // A plane with this layermask will get raycasted to, to find the point the player aims towards
    [SerializeField] private LayerMask _aimPlaneMask = 0;

    private Camera _mainCamera = null;
    public Vector3 PositionToAimAt { get; private set; } = Vector3.zero;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateAim();
        UpdateRotation();
    }

    /// <summary>
    /// This method finds the point the players mouse is aiming at and updates that value in this component.
    /// </summary>
    private void UpdateAim()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running)
            return;

        Ray aimRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(aimRay, out RaycastHit hit, 40, _aimPlaneMask))
        {
            Vector3 positionToAimAtTemp = hit.point;
            // We set the y position to this gameobjects y position to make rotation later easier
            positionToAimAtTemp.y = transform.position.y;
            PositionToAimAt = positionToAimAtTemp;
        }
    }

    /// <summary>
    /// This method updates the players rotation to aim where the mouse is pointing towards
    /// </summary>
    private void UpdateRotation()
    {
        transform.LookAt(PositionToAimAt, Vector3.up);
    }
}
