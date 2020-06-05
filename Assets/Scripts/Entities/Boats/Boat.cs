using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boat : Entity
{
    public int id;
    public Collider2D deck;
    public Dictionary<BoatEntityType, Dictionary<int, BoatEntity>> boatEntitiesByType;

    public void Init(int id)
    {
        this.id = id;
        this.InitBoatEntities();
    }

    private void InitBoatEntities()
    {
        this.boatEntitiesByType = new Dictionary<BoatEntityType, Dictionary<int, BoatEntity>>();

        foreach (BoatEntityType boatEntityType in Enum.GetValues(typeof(BoatEntityType)).Cast<BoatEntityType>())
        {
            this.boatEntitiesByType[boatEntityType] = new Dictionary<int, BoatEntity>();
        }
    }
}
