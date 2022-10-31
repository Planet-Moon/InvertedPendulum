using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionWheelControl : PID
{
    public GameObject reactionWheel;
    public GameObject frame;
    public float angleSetpoint;
    public float angle;

    public float error;

    public float k;
    public float motorVel;
    public float motorForce;

    private HingeJoint hinge;
    private JointMotor motor;

    [System.Serializable]
    public struct PIDParams
    {
        public float kp;
        public float ki;
        public float kd;
    }

    public PIDParams standupParams;
    public PIDParams balanceParams;
    public float pidSwitchAngle;

    public override float getError()
    {

        angle = frame.transform.eulerAngles.z;
        error = Mathf.DeltaAngle(angle, angleSetpoint);
        return error;
    }

    private void FixedUpdate()
    {
        float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(frame.transform.eulerAngles.z, angleSetpoint));
        if (deltaAngle < pidSwitchAngle)
        {
            kp = balanceParams.kp;
            ki = balanceParams.ki;
            kd = balanceParams.kd;
        }
        else if( deltaAngle > pidSwitchAngle+5)
        {
            kp = standupParams.kp;
            ki = standupParams.ki;
            kd = standupParams.kd;
        }
        motor.force = motorForce;
        motor.freeSpin = false;
        motorVel = k * calculate();
        motor.targetVelocity = motorVel;
        hinge.motor = motor;
    }

    // Start is called before the first frame update
    void Start()
    {
        hinge = reactionWheel.GetComponent<HingeJoint>();
        motor = hinge.motor;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
