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
    private Quaternion targetRot;

    bool check = true;
    int tmp = 0;

    private void Update()
    {
        /*tmp += 1;
        if (check && tmp >= 1000)
        {
            print("Follow trans pos " + this.followTransform.position);
            print("Follow trans fow " + this.followTransform.forward * this.distance);
            print("transform pos " + transform.position);

            print("===================================");
            print("Target pos 1 " + (this.followTransform.position - this.followTransform.forward * this.distance));
            print("Target pos 2 " + ((this.followTransform.position - this.followTransform.forward * this.distance) + ((transform.position - this.targetPos) * this.delayTime)));


            check = false;
        }*/

        this.targetPos = this.followTransform.position - this.followTransform.forward * this.distance;
        /*this.transform.rotation = this.followTransform.rotation;*/
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.followTransform.rotation, Time.deltaTime * 5);

        //this.targetPos += (transform.position - this.targetPos) * this.delayTime;
        transform.position = Vector3.Lerp(transform.position, this.targetPos, Time.deltaTime * this.moveStep);
    }
}
