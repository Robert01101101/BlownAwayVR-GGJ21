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
        private float moveSpeed = 2f;

        [SerializeField] 
        private float rotateSpeed = 100f;

        private void MoveWorld(Vector2 scrollDir)
        {
            // Move the plane
            scrollingPlaneWater.Move(scrollDir, moveSpeed*Time.deltaTime);
            
            // Move all the objects
            List<ScrollingObject> scrollingObjects = ScrollingObjectRegistry.GetScrollingObjects();
            foreach (ScrollingObject sceneObject in scrollingObjects)
            {
                // This conversion is important - maintains the sense of coherency between offsetted texture and moved object
                float moveObjectSpeed = moveSpeed / scrollingPlaneWater.WaterTile;
                sceneObject.Move(scrollDir, moveObjectSpeed * Time.deltaTime);
            }
        }

        private void Update()
        {
            // This will use boat's forward direction and therefore component must be on the boat
            Vector2 scrollDir = new Vector2(transform.forward.x, transform.forward.z);
            if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                MoveWorld(scrollDir);
            }

            if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                MoveWorld(-scrollDir);
            }

            if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -0.5f)
            {
                transform.Rotate(transform.up, -rotateSpeed*Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > 0.5f)
            {
                transform.Rotate(transform.up, rotateSpeed*Time.deltaTime);
            }
        }
    }
}
