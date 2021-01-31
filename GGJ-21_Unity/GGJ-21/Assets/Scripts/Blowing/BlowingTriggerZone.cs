using System;
using UnityEngine;

namespace Blowing
{
    public class BlowingTriggerZone : MonoBehaviour
    {
        [SerializeField]
        private float thresholdToGenerate = 0.2f;

        [SerializeField] 
        private float distance = 1f;

        [SerializeField] 
        private float radius = 1f;
        
        private void Update()
        {
            if (ExhaleInput.isExhaling && ExhaleInput.strength > thresholdToGenerate)
            {
                RaycastHit[] allHits = Physics.SphereCastAll(transform.position, radius, transform.forward, distance);
                foreach (RaycastHit hit in allHits)
                {
                    //Assign component
                    GameObject hitGO = hit.transform.gameObject;
                    if (hitGO.GetComponent<AffectedByBlow>() == null)
                    {
                        hitGO.AddComponent<AffectedByBlow>();
                    }
                }
            }
        }
    }
}
