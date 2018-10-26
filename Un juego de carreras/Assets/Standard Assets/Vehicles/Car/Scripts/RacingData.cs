using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingData
{
    [Serializable]
    public struct CarStatusContainer
    {
        public List<CarStatus> carMovement;
        public float Time;
    }
    [Serializable]
    public struct CarStatus
    {
        public ObjectTransform Car;
        public ObjectTransform WheelFrontRight;
        public ObjectTransform WheelFrontLeft;
        public ObjectTransform WheelBackRight;
        public ObjectTransform WheelBackLeft;
        public ObjectTransform Camera;
        public float AccelInput;
        public float Revs;
    }
    [Serializable]
    public struct ObjectTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
    }
}
