using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController main;

    public Transform positionTarget = null;
    public Transform rotateTarget = null;
    public float rotationSpeed = 1;
    public float moveSpeed = 1;
    public bool rotate;

    public void Awake()
    {
        CameraController.main = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (positionTarget != null)
        {
            float z = this.transform.position.z;
            Vector3 position = Vector3.Lerp(transform.position, this.positionTarget.position, this.moveSpeed * Time.deltaTime); ;
            position.z = z;
            this.transform.position = position;
        }
        


        if (rotateTarget != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTarget.transform.rotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
        }
    }
}
