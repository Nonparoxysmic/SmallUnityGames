using System.Collections;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return null;

        // TODO: Initialize this menu.
    }
}
