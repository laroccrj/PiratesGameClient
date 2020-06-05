using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    USE = 1,
    REPAIR
}

public enum InteractableType
{
    MOOUNTABLE,
    WALL
}

public interface Interactable
{

    InteractableType GetInteractableType();

    int GetId();

    InteractionType[] GetPossibleInteractionTypes();
}
