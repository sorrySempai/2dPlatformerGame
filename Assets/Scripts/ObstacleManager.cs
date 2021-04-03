using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    public int dmg = 3;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Unit unit = col.GetComponent<Unit>();

        if (unit && unit is PlayerMovement)
        {
            unit.ReceiveDamage(dmg);
        }
    }
}
