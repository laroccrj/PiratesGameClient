using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 TargetPosition = Vector3.zero;
    private float syncSpeed = 20f;
    public bool usingLocalPosition;
    public bool entityMovement = true;

    protected void Update()
    {
        if (!entityMovement)
            return;

        if (usingLocalPosition)
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, this.TargetPosition, this.syncSpeed * Time.deltaTime);
        else
            this.transform.position = Vector3.Lerp(transform.position, this.TargetPosition, this.syncSpeed * Time.deltaTime);
    }
}
