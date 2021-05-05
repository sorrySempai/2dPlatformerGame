using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTrap : MonoBehaviour
{
    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wall3;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            wall1.SetActive(false);
            wall2.SetActive(false);
            wall3.SetActive(false);
        }
    }
}
