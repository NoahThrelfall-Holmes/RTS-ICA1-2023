using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlane : MonoBehaviour
{
    SphereManager sphereManager;
    void Start()
    {
        sphereManager = GameObject.FindGameObjectWithTag("SphereManager").GetComponent<SphereManager>();
        sphereManager.planes.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
