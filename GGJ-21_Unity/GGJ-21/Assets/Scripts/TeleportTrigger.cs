using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//Teleport to next scene OR another location within this scene (if assigned)
//

public class TeleportTrigger : MonoBehaviour
{
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (target != null)
            {
                SceneLoader.instance.Teleport(target.position);
            }
            else
            {
                SceneLoader.instance.LoadNextScene();
            }
        }
        
    }
}
