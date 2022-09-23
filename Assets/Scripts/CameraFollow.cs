using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    //SmoothFactor: The lower the smoother and vice versa
    [Range(1, 10)]
    [SerializeField] private float smoothFactor;
    [SerializeField] private Vector3 offset;
    private void FixedUpdate()
    {
        if (target != null)
        {
            FollowCamera();
        }
    }

    private void FollowCamera()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.localPosition, targetPos, smoothFactor * Time.fixedDeltaTime);
        transform.localPosition = smoothPos;
    }
}
