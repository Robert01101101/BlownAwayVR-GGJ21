using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpTestScript : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = transform.position += transform.forward / 10;
        }
    }
}
