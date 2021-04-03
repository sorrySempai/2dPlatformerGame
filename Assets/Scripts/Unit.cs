using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public virtual void ReceiveDamage(int dmg)
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
