using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    USE = 1,
    REPAIR
}

public interface Interactable
{
    // Right now you can only interact with boat entities, can abstract in the future
    BoatEntityType BoatEntityType
    {
        get;
    }

    BoatEntity GetBoatEntity();

    InteractionType[] GetPossibleInteractionTypes();
}
