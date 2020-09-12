using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireFollowPlayer : MonoBehaviour
{

    public Transform target;
    Vector2 targetpos;
    Vector3 refVel = Vector3.zero;
    public float smoothTime = 0.3f;

    void Update()
    {
        Vector3 tempPos;
        targetpos = Camera.main.WorldToScreenPoint(target.position);
        tempPos = Vector3.SmoothDamp(transform.position, targetpos, ref refVel, smoothTime);
        transform.position = tempPos;
    }
}