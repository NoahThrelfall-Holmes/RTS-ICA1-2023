using UnityEngine;
using System.Collections.Generic;

public class SphereManager : MonoBehaviour
{
    public List<CustomSphere> spheres;
    public List<CustomPlane> planes;

    void FixedUpdate()
    {
        foreach (var sphere in spheres)
        {
            sphere.Move();
        }

        for (int i = 0; i < spheres.Count; i++)
        {
            for (int j = i + 1; j < spheres.Count; j++)
            {
                var sphere1 = spheres[i];
                var sphere2 = spheres[j];

                // Relative velocity
                Vector3 relativeVelocity = sphere1.velocity - sphere2.velocity;
                // Relative position
                Vector3 relativePosition = sphere1.transform.position - sphere2.transform.position;

                // Coefficients for the quadratic equation
                float a = Vector3.Dot(relativeVelocity, relativeVelocity);
                float b = 2 * Vector3.Dot(relativeVelocity, relativePosition);
                float c = Vector3.Dot(relativePosition, relativePosition) - (sphere1.GetRadius() + sphere2.GetRadius()) * (sphere1.GetRadius() + sphere2.GetRadius());

                // Calculate the discriminant
                float discriminant = b * b - 4 * a * c;

                // Check if there are real solutions
                if (discriminant >= 0)
                {
                    // Compute the two solutions of t
                    float t1 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
                    float t2 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);

                    // Check if the collisions occur within the frame
                    if ((t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1))
                    {
                        float tCollision = Mathf.Min(t1, t2);
                        if (tCollision >= 0 && tCollision <= 1)
                        {
                            // Check if the collision is within this physics update
                            if (tCollision <= Time.fixedDeltaTime)
                            {
                                // Move spheres to the point just before collision
                                sphere1.transform.position += sphere1.velocity * tCollision;
                                sphere2.transform.position += sphere2.velocity * tCollision;

                                // Resolve the collision
                                sphere1.ResolveCollision(sphere2, tCollision);
                                //sphere2.ResolveCollision(sphere1, tCollision);

                                // Log the collision time
                                Debug.Log("Collision at t=" + tCollision);
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < spheres.Count; i++)
        {
            for (int j = 0; j < planes.Count; j++)
            {
                Vector3 planeNormal = -planes[j].transform.up;
                Vector3 pointOnPlane = planes[j].transform.position; // Assuming this is 'k'
                Vector3 spherePosition = spheres[i].transform.position;
                Vector3 P = spherePosition - pointOnPlane;

                // Check if sphere is moving towards the plane
                if (Vector3.Dot(spheres[i].velocity, planeNormal) > 0)
                {
                    float d = Vector3.Dot(P, planeNormal) - spheres[i].GetRadius();
                    if (d <= 0)
                    {
                        // Calculate time of collision
                        float numerator = Vector3.Dot(-P, planeNormal) - spheres[i].GetRadius();
                        float denominator = Vector3.Dot(spheres[i].velocity, planeNormal);
                        float tCollision = numerator / denominator;

                        if (tCollision >= 0 && tCollision <= Time.fixedDeltaTime)
                        {
                            spheres[i].transform.position += spheres[i].velocity * tCollision;
                            spheres[i].ResolvePlaneCollision(planeNormal, d, tCollision);
                        }
                    }
                }
            }
        }
    }
}
