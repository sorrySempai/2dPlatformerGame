using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawManager : ObstacleManager
{
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 5f));
    }
}
