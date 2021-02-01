using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScrollingPlane
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField]
        private ScrollingActorWater actorWater = default;
        
        [SerializeField]
        private ObstacleCollection obstacleCollection = default;

        [SerializeField]
        private float defaultTimePeriod = default;

        [SerializeField] 
        private float minDistance = 10f;

        [SerializeField] 
        private float maxDistance = 20f;

        [SerializeField] 
        private float sideStepRadius = 20f;

        private float TimePeriod => defaultTimePeriod / actorWater.SpeedMultiplier;
        
        private float prevTime;

        private float LargestAxis(Vector3 vec)
        {
            if (vec.x > vec.y && vec.x > vec.z)
            {
                return vec.x;
            }

            if (vec.y > vec.x && vec.y > vec.z)
            {
                return vec.y;
            }

            return vec.z;
        }

        private bool OverlapsContainAnotherScrollingObject(Collider[] overlaps)
        {
            foreach (Collider overlap in overlaps)
            {
                if (overlap.GetComponent<ScrollingObject>() != null)
                {
                    return true;
                }
            }

            return false;
        }

        private bool OverlapsWithOtherScrollingObjectAtPos(GameObject objectToPlace, Vector3 position)
        {
            Bounds obstacleBounds = objectToPlace.GetComponentInChildren<MeshRenderer>().bounds;
            float maxMeasure = LargestAxis(obstacleBounds.size);
            Collider[] overlaps = Physics.OverlapSphere(position, maxMeasure);
            return OverlapsContainAnotherScrollingObject(overlaps);
        }

        private Vector3 GenerateRandomObstaclePosition()
        {
            Vector3 actorForwardDir = actorWater.transform.forward;
            Vector3 nextObstaclePos = actorWater.transform.position + actorForwardDir * Random.Range(minDistance, maxDistance);
            nextObstaclePos += actorWater.transform.right * Random.Range(-sideStepRadius, sideStepRadius);
            return nextObstaclePos;
        }

        private void Awake()
        {
            prevTime = Time.time;
        }

        private void Update()
        {
            float timeDiff = Time.time - prevTime;
            if (timeDiff < TimePeriod)
            {
                return;
            }
            
            GameObject randomObstacleToSpawn = obstacleCollection.RandomObstacle;
            Vector3 nextObstaclePos = GenerateRandomObstaclePosition();
            int attempts = 10;
            for (int i = 0; i < attempts; i++)
            {
                if (OverlapsWithOtherScrollingObjectAtPos(randomObstacleToSpawn, nextObstaclePos) == false)
                {
                    GameObject obstacle = Instantiate(randomObstacleToSpawn, nextObstaclePos, Quaternion.identity);
                    
                    //We are using regular objects as obstacles -> assign components to them
                    obstacle.GetComponent<MeshRenderer>().material = obstacleCollection.MaterialToAssign;
                    obstacle.AddComponent<ScrollingWaterMaterialObject>();
                    obstacle.AddComponent<ScrollingObject>();

                    prevTime = Time.time;
                    return;
                }

                nextObstaclePos = GenerateRandomObstaclePosition();
            }
            
            
            Debug.Log("Failed to place an obstacle");
            prevTime = Time.time;


        }
    }
}
