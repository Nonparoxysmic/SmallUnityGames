using System.Collections;
using UnityEngine;

public abstract class ActionInstruction : MonoBehaviour
{
    public static Battle Battle { get; set; }

    public static CreatureAction Action { get => Battle.CurrentAction; }

    public abstract IEnumerator Execute();
}
