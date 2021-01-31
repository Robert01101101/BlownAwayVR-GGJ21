using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollingPlane
{
    /// <summary>
    /// This component must be placed on the boat
    /// </summary>
    public class ScrollingActorWater : MonoBehaviour
    {
        [SerializeField] 
        private ScrollingPlaneWater scrollingPlaneWater = default;

        [SerializeField] 
        private float idleSpeed = 2f;

        [SerializeField] 
        private float maxAcceleratedSpeed = 10f;

        [SerializeField] 
        private float accelerationTime = 2f;

        [SerializeField] 
        private float deccelerationValue = 0.5f;

        [SerializeField] 
        private float rotateSpeed = 100f;

        private float currentMoveSpeed;
        public float SpeedMultiplier => currentMoveSpeed / idleSpeed;

        private float accelerationStartSpeed;
        private float expectedAccelerationTime;
        private float accelerationStart;
        private bool isAccelerating;

        private void Awake()
        {
            currentMoveSpeed = idleSpeed;
        }

        private void MoveWorld(Vector2 scrollDir)
        {
            // Move the plane
            scrollingPlaneWater.Move(scrollDir, currentMoveSpeed*Time.deltaTime);
            
            // Move all the objects
            List<ScrollingObject> scrollingObjects = ScrollingObjectRegistry.GetScrollingObjects();
            foreach (ScrollingObject sceneObject in scrollingObjects)
            {
                // This conversion is important - maintains the sense of coherency between offsetted texture and moved object
                float moveObjectSpeed = currentMoveSpeed / scrollingPlaneWater.WaterTile;
                sceneObject.Move(scrollDir, moveObjectSpeed * Time.deltaTime);
            }
        }

        private void InputAndMoveTick()
        {
            // This will use boat's forward direction and therefore component must be on the boat
            Vector2 scrollDir = new Vector2(transform.forward.x, transform.forward.z);
            MoveWorld(scrollDir);

            if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -0.5f)
            {
                transform.Rotate(transform.up, -rotateSpeed*Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > 0.5f)
            {
                transform.Rotate(transform.up, rotateSpeed*Time.deltaTime);
            }
        }

        private void AcceleratingTick()
        {
            if (isAccelerating == false)
            {
                return;
            }

            float timePassed = Time.time - accelerationStart;
            float value = Mathf.Min(timePassed / expectedAccelerationTime, 1f);

            currentMoveSpeed = Mathf.Lerp(accelerationStartSpeed, maxAcceleratedSpeed, value);

            if (value >= 1f)
            {
                isAccelerating = false;
            }
        }

        private void DecceleratingTick()
        {
            if (isAccelerating)
            {
                return;
            }

            currentMoveSpeed -= deccelerationValue * Time.deltaTime;
            currentMoveSpeed = Mathf.Max(currentMoveSpeed, idleSpeed);
        }

        private void Update()
        {
            if (ExhaleInput.strength > 0.2f && isAccelerating == false)
            {
                //Accelerate linearly and therefore can predict the time
                accelerationStartSpeed = currentMoveSpeed;
                expectedAccelerationTime = accelerationTime * (1 - Mathf.InverseLerp(idleSpeed, maxAcceleratedSpeed, currentMoveSpeed));
                Debug.Log("Acceleration: "+expectedAccelerationTime);
                accelerationStart = Time.time;
                isAccelerating = true;
            }
            
            AcceleratingTick();
            DecceleratingTick();
            InputAndMoveTick();
        }
    }
}
