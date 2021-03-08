using System;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public string testFacingDirection;

    void Update()
    {
        float horz = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        float vert = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        float horzMag = Math.Abs(horz);
        float vertMag = Math.Abs(vert);
        if (vertMag > horzMag)
        {
            if (vert >= 0) testFacingDirection = "North";
            else testFacingDirection = "South";
        }
        else
        {
            if (horz >= 0) testFacingDirection = "East";
            else testFacingDirection = "West";
        }
    }
}
