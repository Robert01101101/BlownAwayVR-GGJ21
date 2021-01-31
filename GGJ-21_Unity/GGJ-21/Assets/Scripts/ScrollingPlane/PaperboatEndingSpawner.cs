using System;
using UnityEngine;

namespace ScrollingPlane
{
    public class PaperboatEndingSpawner : MonoBehaviour
    {
        [SerializeField] 
        private ScrollingActorWater actorWater = default;
        
        [SerializeField] 
        private float gameTime = 30f;

        [SerializeField] 
        private float gapBeforeEndingSpawn = 3f;

        private float TotalTimeBeforeEnding => gameTime + gapBeforeEndingSpawn;
        
        [SerializeField]
        private ObstacleSpawner obstacleSpawner = default;
        
        [SerializeField]
        private GameObject endingPrefab = default;

        [SerializeField] 
        private float spawnDistance = 20f;

        private float startTime;

        private void Awake()
        {
            startTime = Time.time;
        }

        private void Update()
        {
            float passedTime = Time.time - startTime;
            if (passedTime >= gameTime)
            {
                obstacleSpawner.enabled = false;
            }

            if (passedTime > TotalTimeBeforeEnding)
            {
                Vector3 actorForwardDir = actorWater.transform.forward;
                Vector3 nextObstaclePos = actorWater.transform.position + actorForwardDir * spawnDistance;
                Instantiate(endingPrefab, nextObstaclePos, Quaternion.LookRotation(actorForwardDir));
                enabled = false;
            }
        }
    }
}
