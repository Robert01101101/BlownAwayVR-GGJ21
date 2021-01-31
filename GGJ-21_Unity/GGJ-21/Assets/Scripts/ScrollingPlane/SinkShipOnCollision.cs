using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScrollingPlane
{
    public class SinkShipOnCollision : MonoBehaviour
    {
        [SerializeField] 
        private int hitPoints = 3;

        [SerializeField] 
        private float submergingSpeed = 0.5f;

        [SerializeField] 
        private float reloadTime = 3f;
        
        [SerializeField]
        private Material materialToAssignOnSink = default;

        private int currentHitPoints;
        private float sinkStartTime;
        private bool collisionEnabled;

        private void Awake()
        {
            currentHitPoints = hitPoints;
            collisionEnabled = true;
            enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (collisionEnabled == false)
            {
                return;
            }
            
            if (other.gameObject.GetComponent<ScrollingObject>() == false)
            {
                return;
            }

            bool isDeadly = other.gameObject.GetComponent<ScrollingObjectDeadly>();
            if (isDeadly)
            {
                currentHitPoints -= 100;
            }
            else
            {
                Destroy(other.gameObject);
                currentHitPoints--;
            }
         

            if (currentHitPoints <= 0)
            {
                //Assign the ship material
                foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.material = materialToAssignOnSink;
                }
                
                //Enable compatibility with water
                gameObject.AddComponent<ScrollingWaterMaterialModifier>();

                //Start sinking
                collisionEnabled = false;
                sinkStartTime = Time.time;
                enabled = true;
            }
        }

        private void Update()
        {
            float timePassed = Time.time - sinkStartTime;
            if (timePassed > reloadTime)
            {
                enabled = false;
                SceneLoader.instance.LoadScene(SceneLoader.PAPERBOAT_INTRO);
                return;
            }
            
            transform.Translate(Vector3.down*submergingSpeed*Time.deltaTime);
        }
    }
}
