using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLevelLoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) SceneLoader.instance.LoadNextScene();
    }
}
