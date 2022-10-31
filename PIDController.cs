using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PID : MonoBehaviour
{
    public float kp;
    public float ki;
    public float kd;

    public float dt = 1;

    public float output_min = -1;
    public float output_max = 1;
    public bool antiWindup = false;

    public float _pError = 0;
    public float _intError = 0;
    public float _dError = 0;

    public float _lastError = 0;
    public float _output = 0;

    private bool first_run = true;

    private float _intStorage = 0;

    public float getIntError()
    {
        return _intError;
    }

    public void resetIntError()
    {
        _intError = 0;
    }

    public abstract float getError();

    public float calculate()
    {
        float e = getError();
        _pError = kp * e;
        float addIntError = e * dt;
        _intStorage += addIntError;
        if (antiWindup)
        {
            if (_intStorage > output_max)
            {
                _intStorage = output_max;
            }
            else if (_intStorage < output_min)
            {
                _intStorage = output_min;
            }
        }
        _intError = ki * _intStorage;
        float e_dot = (e - _lastError) / dt;
        _dError = kd * e_dot;
        if (first_run)
        {
            _dError = 0;
            first_run = false;
        }
        float output = _pError + _intError + _dError;
        if (antiWindup)
        {
            if (output > output_max)
            {
                output = output_max;
            }
            else if (output < output_min)
            {
                output = output_min;
            }
        }
        _lastError = e;
        _output = output;
        return output;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
