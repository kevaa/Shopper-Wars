using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class Follow : MonoBehaviour
{
    [SerializeField] bool delay;
    [SerializeField] Transform followTarget;
    [SerializeField] float smoothSpeed = .125f;
    Vector3 camOffset = Vector3.up * 19f;

    float minDistToStartFollow = .35f;
    float targetDistMultiplier;

    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var targetPos = followTarget.position + camOffset;
        if (delay)
        {
            var thisToTargetMultiplier = (targetPos - transform.position).magnitude;
            if (thisToTargetMultiplier > minDistToStartFollow)
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * thisToTargetMultiplier * Time.deltaTime);
        }
        else
        {
            transform.position = targetPos;
        }
    }

}
