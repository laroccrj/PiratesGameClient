﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoatEntityType
{
    WALL = 1,
    CANNON,
    STEERING_WHEEL,
}

public abstract class BoatEntity : MonoBehaviour
{
    public int id;

    public abstract BoatEntityType BoatEntityType
    {
        get;
    }

    public abstract void ReadDataFromPacket(Packet packet);
}
