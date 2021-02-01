using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Trigger
{
    left,
    right,
    none
}

public class Rudder : MonoBehaviour
{
    public Transform handDetection;
    public Transform rotationAxisTransform;
    Transform hand;
    Trigger currentTrigger;

    private float maxDistance = .5f;
    private Vector3 rudderNeutralPosition;


    private bool beingHeld = false;

    [System.NonSerialized]
    public float direction; //-1 - L, 1 = R

    private void Start()
    {
        rudderNeutralPosition = transform.forward;
        //rotationAxis = rotationAxisTransform.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        DetectTrigger();

        //Debug
        Debug.Log(handDetection.localPosition.x);
    }

    private void DetectTrigger()
    {
        currentTrigger = Trigger.none;

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0)
        {
            currentTrigger = Trigger.left;
            MeasureDistance();
        }
        else if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0)
        {
            currentTrigger = Trigger.right;
            MeasureDistance();
        }
        else
        {
            if (beingHeld) ReturnToNeutral();//StartCoroutine(ApproachNeutral());
        }
    }

    private void MeasureDistance()
    {
        string handString = (currentTrigger == Trigger.left) ? "LeftHandAnchor" : "RightHandAnchor" ;
        hand = GameObject.Find(handString).transform;
        float distance = Vector3.Distance(handDetection.position, hand.position);
        if (distance < maxDistance)
        {
            Steer();
        } else
        {
            hand = null;
            if (beingHeld) ReturnToNeutral();//StartCoroutine(ApproachNeutral());
        }
    }

    private void Steer()
    {
        beingHeld = true;
        StopAllCoroutines();

        //Face the hand
        transform.LookAt(hand);

        //output steering value between -1 to 1
        Vector3 crossProduct = Vector3.Cross(rudderNeutralPosition, transform.forward);
        direction = mapVal(crossProduct.z, .4f, -.4f, -1, 1, true);

        DebugCanvas.DebugLog(direction.ToString());
    }

    //map s from range a1-a2 to b1-b2
    //Modified, originally from: https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    public static float mapVal(float s, float a1, float a2, float b1, float b2, bool capped = false)
    {
        float output = b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        if (!capped)
        {
            return output;
        }
        else
        {
            //Option to limit output to target range.
            return ((output < b1 ? b1 : output) > b2 ? b2 : output);
        }
    }

    private void ReturnToNeutral()
    {
        Quaternion rotation = Quaternion.LookRotation(rudderNeutralPosition, Vector3.up);
        transform.rotation = rotation;

        direction = 0;

        DebugCanvas.DebugLog(direction.ToString());
    }

    ////////////////// Return to neutral position when let go
    /*
    IEnumerator ApproachNeutral()
    {
        beingHeld = false;

        Quaternion targetDir = Quaternion.LookRotation(rudderNeutralPosition);

        while (Quaternion.Angle(Quaternion.LookRotation(transform.forward), targetDir) > 0.01f)
        {
            Quaternion.RotateTowards(Quaternion.LookRotation(transform.forward), targetDir, Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

    }*/
}
