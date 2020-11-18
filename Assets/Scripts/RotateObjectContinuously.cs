using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectContinuously : MonoBehaviour
{
    [SerializeField] private float rotationRate = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //Vector3 currentRot = this.gameObject.transform.rotation.eulerAngles;
        //print(currentRot);
        this.gameObject.transform.Rotate(new Vector3(0f, rotationRate, 0f));
    }
}
