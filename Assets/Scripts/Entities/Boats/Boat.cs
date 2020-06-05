using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : Entity
{
    public int id;
    public Collider2D deck;
    public Dictionary<int, Mountable> mountables = new Dictionary<int, Mountable>();
    public Dictionary<int, Wall> walls = new Dictionary<int, Wall>();
}
