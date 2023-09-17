using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SnakeCtrl : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float bodySpeed = 5f;
    public float steerSpeed = 180f;

    public GameObject bodyPrefab;
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionHistorys = new List<Vector3>();

    public int gap = 10;

    private void Start()
    {
        this.GrowSnake();
        this.GrowSnake();
        this.GrowSnake();
        this.GrowSnake();
        this.GrowSnake();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
    }

    private void Update()
    {
        // move forward
        this.transform.position += transform.forward * this.moveSpeed * Time.deltaTime;

        // steering
        float steerDirection = Input.GetAxis("Horizontal");
        this.transform.Rotate(Vector3.up * steerDirection * this.steerSpeed * Time.deltaTime);

        // store position history
        this.positionHistorys.Insert(0, this.transform.position);

        // move body parts
        int index = 0;
        foreach(var body in this.bodyParts)
        {
            Vector3 point = this.positionHistorys[Mathf.Min(index * gap, this.positionHistorys.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * this.bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }
    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(this.bodyPrefab);
        this.bodyParts.Add(body);
    }
}
