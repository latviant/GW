using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private float _sidewaysMption = 0.0f;

    public float sidewaysMotion
    {
        get
        {
            return _sidewaysMption;
        }
    }

    private void Update()
    {
        Vector3 accel = Input.acceleration;

        _sidewaysMption = accel.x;
    }
}

