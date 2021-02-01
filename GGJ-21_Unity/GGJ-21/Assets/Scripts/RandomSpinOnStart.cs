using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpinOnStart : MonoBehaviour
{
    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddTorque(Random.insideUnitSphere*50f, ForceMode.Impulse);
    }
}
