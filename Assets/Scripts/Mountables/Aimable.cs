using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimable : Mountable
{
    public float rotationSpeed = 1;
    public float maxRotation = 60;

    private Vector3 rotationCeiling = new Vector3();
    private Vector3 rotationFloor = new Vector3();
    private ProjectileLauncher launcher = null;
    public override Mountables MountableType => Mountables.STEERING_WHEEL;

    public override void OnDismount()
    {

    }

    public override void OnMount()
    {

    }
    public void Awake()
    {
        this.launcher = gameObject.GetComponent<ProjectileLauncher>();
        this.rotationCeiling.z = transform.eulerAngles.z + maxRotation;
        this.rotationFloor.z = transform.eulerAngles.z - maxRotation;

        if (this.rotationCeiling.z > 360)
            this.rotationCeiling.z -= 360;

        if (this.rotationFloor.z < 0)
            this.rotationFloor.z += 360;
    }

    public void Update()
    {
        if (!this.mounted)
            return;

        Quaternion rotation = new Quaternion();


        if (Input.GetKey(KeyCode.A))
        {
            rotation = Quaternion.Euler(this.rotationCeiling);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotation = Quaternion.Euler(this.rotationFloor);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
