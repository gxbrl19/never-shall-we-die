using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
    private float _horizontal;
    private float _vertical;
    private float _accelerate;
    private bool _interact;
    private bool _submarine;
    private bool _propulsion;
    private bool _cannon;
    private bool _cancel;

    ShipOpenWorld _ship;

    #region Properties

    public float horizontal
    {
        get { return _horizontal; }
        set { _horizontal = value; }
    }

    public float vertical
    {
        get { return _vertical; }
        set { _vertical = value; }
    }

    public float accelerate
    {
        get { return _accelerate; }
        set { _accelerate = value; }
    }

    public bool interact
    {
        get { return _interact; }
        set { _interact = value; }
    }

    public bool submarine
    {
        get { return _submarine; }
        set { _submarine = value; }
    }

    public bool propulsion
    {
        get { return _propulsion; }
        set { _propulsion = value; }
    }

    public bool cannon
    {
        get { return _cannon; }
        set { _cannon = value; }
    }

    public bool cancel
    {
        get { return _cancel; }
        set { _cancel = value; }
    }
    #endregion

    void Start()
    {
        _ship = GetComponent<ShipOpenWorld>();
    }

    public void Horizontal(InputAction.CallbackContext callback)
    {
        _horizontal = callback.ReadValue<float>();
    }

    public void Vertical(InputAction.CallbackContext callback)
    {
        _vertical = callback.ReadValue<float>();
    }

    public void Accelerate(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            _accelerate = 1f;
        }

        if (callback.canceled)
        {
            _accelerate = 0f;
        }
    }

    public void Cancel(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {

        }
    }

    public void Interact(InputAction.CallbackContext callback)
    {
        if (propulsion || _ship._submarine || cannon) { return; }

        if (callback.started)
        {
            _interact = true;
        }

        if (callback.canceled)
        {
            _interact = false;
        }
    }

    public void Submarine(InputAction.CallbackContext callback)
    {
        if (propulsion || _ship._submarine) { return; }

        if (callback.started && ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Submarine))
        {
            _submarine = true;
        }

        if (callback.canceled)
        {
            _submarine = false;
        }
    }

    public void Propulsion(InputAction.CallbackContext callback)
    {
        if (_ship._submarine) { return; }

        if (callback.started && ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Propulsion))
        {
            _propulsion = true;
        }

        if (callback.canceled)
        {
            _propulsion = false;
        }
    }

    public void Cannon(InputAction.CallbackContext callback)
    {
        if (propulsion || _ship._submarine) { return; }

        if (callback.started && ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Cannon))
        {
            _cannon = true;
        }

        if (callback.canceled)
        {
            _cannon = false;
        }
    }
}
