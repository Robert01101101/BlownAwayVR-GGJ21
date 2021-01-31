using UnityEngine;
using Random = UnityEngine.Random;

namespace ScrollingPlane
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField]
        private ScrollingActorWater actorWater = default;
        
        [SerializeField]
        private GameObject obstaclePrefab = default;

        [SerializeField]
        private float timePeriod = default;

        [SerializeField] 
        private float minDistance = 10f;

        [SerializeField] 
        private float maxDistance = 20f;

        [SerializeField] 
        private float sideStepRadius = 20f;
        
        private float prevTime;

        private void Awake()
        {
            prevTime = Time.time;
        }

        private void Update()
        {
            float timeDiff = Time.time - prevTime;
            if (timeDiff < timePeriod)
            {
                return;
            }
            
            Vector3 actorForwardDir = actorWater.transform.forward;
            Vector3 nextObstaclePos = actorWater.transform.position + actorForwardDir * Random.Range(minDistance, maxDistance);
            nextObstaclePos += actorWater.transform.right * Random.Range(-sideStepRadius, sideStepRadius);
            Instantiate(obstaclePrefab, nextObstaclePos, Quaternion.identity);

            prevTime = Time.time;
        }
    }
}
