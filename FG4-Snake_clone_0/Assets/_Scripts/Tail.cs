using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public Transform networkedOwner;
    public Transform followTransform;

    [SerializeField] private float delayTime = 0.1f;
    [SerializeField] private float distance = 0.3f;
    [SerializeField] private float moveStep = 10f;

    private Vector3 targetPos;

    bool check = true;

    private void Update()
    {
        if (check)
        {
            print("Follow transform " + this.followTransform.position);
            print("Follow transform forward " + this.followTransform.forward);
            print("Vector forward " + Vector3.forward);
            check = false;
        }

        this.targetPos = this.followTransform.position - this.followTransform.forward * this.distance;
        //this.targetPos = this.followTransform.position - this.followTransform.forward;
        this.targetPos += (transform.position - this.targetPos) * this.delayTime;
        transform.position = Vector3.Lerp(transform.position, this.targetPos, Time.deltaTime * this.moveStep);

    }
}
