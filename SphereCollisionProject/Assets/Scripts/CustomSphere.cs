using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSphere : MonoBehaviour
{
    // pos already known via unity transform
    // need custom mass + velocity
    // we should already know sphere radius (make sure)

    public Vector3 velocity;
    [SerializeField] float mass;
    float radius;

    SphereManager sphereManager;
    void Start()
    {
        sphereManager = GameObject.FindGameObjectWithTag("SphereManager").GetComponent<SphereManager>();
        sphereManager.spheres.Add(this);
        // I like this approach because it allows me to spawn in new spheres during gameplay
        // Drawback: runtime allocation of memory. Possible solution: ObjectPool.

        Vector3 scale = transform.localScale;
        radius = Mathf.Max(scale.x, scale.y, scale.z) / 2.0f;
    }

    // I want to be able to do velocity pos updates here but that might not work.
    // Have movement code here but only call it in the SphereManager
    void Move()
    {

    }

    public float GetMass() { return mass; }
    public float GetRadius() { return radius; }

}
