using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSphere : MonoBehaviour
{
    // pos already known via unity transform
    // need custom mass + velocity
    // we should already know sphere radius (make sure)

    public Vector3 velocity;
    [SerializeField] float mass = 1f;
    [SerializeField] float coefRest = .9f;
    [SerializeField] float gravity = 0.98f;
    float radius;

    SphereManager sphereManager;
    void Start()
    {
        sphereManager = GameObject.FindGameObjectWithTag("SphereManager").GetComponent<SphereManager>();
        sphereManager.spheres.Add(this);
        // I like this approach because it allows me to spawn in new spheres during gameplay
        // Drawback: runtime allocation of memory. Possible solution: ObjectPool.

        // Have to do this to get radius of the unity sphere
        // Uniform scaling is assumed here for correct radius calculation
        Vector3 scale = transform.localScale;
        radius = Mathf.Max(scale.x, scale.y, scale.z) / 2.0f;  
    }

    // I want to be able to do velocity pos updates here but that might not work.
    // Have movement code here but only call it in the SphereManager
    public void Move()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    public void ResolveCollision(CustomSphere other, float tCollision)
    {
        // Gives the normalized vector from the current sphere to the other sphere. i.e. gets the normal of the collision (for spheres at least)
        Vector3 collisionNormal = (other.transform.position - transform.position).normalized;
        //Debug.DrawRay(transform.position, collisionNormal, Color.red, 10f);
        //velocity = Vector3.zero;
        //other.velocity = Vector3.zero;
        //return;

        // Calculate the relative velocity vector (The impulse applied to each sphere to change its velocity is proportional to this relative velocity)
        Vector3 relativeVelocity = velocity - other.velocity;

        // Pre-calculate values to use in equations.
        float massSum = mass + other.GetMass();
        float dotProduct = Vector3.Dot(relativeVelocity, collisionNormal);

        // v1p = v1 - ((2*m2)/(m1+m2)) * (((v1-v2).n)/(n.n)) * n
        Vector3 v1prime = (velocity - (2 * other.GetMass() / massSum) * dotProduct * collisionNormal);
        // v2p = v2 - ((2*m1)/(m1+m2)) * (((v1-v2).n)/(n.n)) * n
        Vector3 v2prime = (other.velocity + (2 * mass / massSum) * dotProduct * collisionNormal);

        velocity = v1prime * coefRest;
        other.velocity = v2prime * coefRest;

        //print("v1: " + velocity);
        //print("v2: " + other.velocity);

        // Tthe remaining time after the collision
        float tRemainder = Time.fixedDeltaTime - tCollision;

        // Move spheres based on their updated velocities for the remaining time in the frame
        transform.Translate(velocity * tRemainder);
        other.transform.Translate(other.velocity * tRemainder);

        //velocity = Vector3.zero;
        //other.velocity = velocity;
    }

    public void ResolvePlaneCollision(Vector3 planeNormal, float distanceToPlane, float tCollision)
    {
        planeNormal.Normalize();

        velocity = velocity - 2 * Vector3.Dot(velocity, planeNormal) * planeNormal;
        velocity *= coefRest;

        // The remaining time after the collision
        float tRemainder = Time.fixedDeltaTime - tCollision;

        // Move sphere based on its updated velocities for the remaining time in the frame
        transform.Translate(velocity * tRemainder);

        //velocity = Vector3.zero;
    }

    public float GetMass() { return mass; }
    public float GetRadius() { return radius; }

    public void SetMass(float _mass) { mass = _mass; }

}
