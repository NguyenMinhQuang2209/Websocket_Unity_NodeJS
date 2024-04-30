using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public string promptMessage = "";
    public void BaseInteract(GameObject target)
    {
        Interact(target);
    }
    public virtual void Interact(GameObject target)
    {

    }
}
