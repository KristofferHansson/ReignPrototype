using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTriggerEnter : MonoBehaviour
{
    [SerializeField] private L01_IntroLevel lvl;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("PCollider"))
        {
            lvl.BeginPlay();
            Destroy(this.gameObject);
        }
    }
}
