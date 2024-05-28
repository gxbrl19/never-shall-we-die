using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
    private float _horizontal;
    private float _vertical;
    private bool _submit;
    private bool _cancel;

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

    public bool submit
    {
        get { return _submit; }
        set { _submit = value; }
    }

    public bool cancel
    {
        get { return _cancel; }
        set { _cancel = value; }
    }
    #endregion

    public void Horizontal(InputAction.CallbackContext callback)
    {
        _horizontal = callback.ReadValue<float>();
    }

    public void Vertical(InputAction.CallbackContext callback)
    {
        _vertical = callback.ReadValue<float>();
    }

    public void Submit(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            _submit = true;
        }

        if (callback.canceled)
        {
            _submit = false;
        }
    }
    
    public void Cancel(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            
        }
    }
}
