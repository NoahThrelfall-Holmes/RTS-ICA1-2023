using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    //get spheres
    public List<CustomSphere> spheres;

    // assuming only 2 spheres for now
    void FixedUpdate()
    {
        
        for (int i = 0; i < spheres.Count - 1; i++)
        {
            print("Hello?");
            for (int j = i + 1; j < spheres.Count; j++)
            {
                // A = get vector between the 2 points
                Vector3 A = spheres[j].transform.position - spheres[i].transform.position;

                // V = velocity vector for sphere 1
                Vector3 V = spheres[i].velocity;

                // d = the distance between the centres of the two spheres at closest approach along path V
                // Calculate the projection of A onto V
                float projection = Vector3.Dot(A, V) / V.magnitude;

                // Calculate the distance of the new line of the triangle formed by A and V
                float d = Mathf.Sqrt(A.sqrMagnitude - projection * projection);

                // DEBUG
                // Calculate the position where d meets V
                Vector3 meetingPosition = spheres[i].transform.position + (projection * V.normalized);
                Debug.DrawLine(spheres[j].transform.position, meetingPosition, Color.blue);

                //Debug.DrawRay(spheres[i].transform.position, V, Color.blue);

                float r1r2 = spheres[i].GetRadius() + spheres[j].GetRadius();
                if (r1r2 > d)
                {
                    float q = Vector3.Angle(A, V);
                    float cosTheta = Mathf.Cos(r1r2 / d);

                    // a2 = b2 + c2 − 2bc cosA
                    float eSquared = d * d + r1r2 * r1r2 - 2 * d * r1r2 * Mathf.Cos(cosTheta);
                    float e = Mathf.Sqrt(eSquared);

                    float distanceAlongV = projection - Mathf.Sqrt(spheres[i].GetRadius() * spheres[i].GetRadius() - e * e);


                    // DEBUG
                    //Debug.DrawLine(spheres[i].transform.position, meetingPosition, Color.red);
                    Debug.DrawLine(spheres[i].transform.position, spheres[j].transform.position, Color.green);
                    Vector3 vectorEndPosition = spheres[i].transform.position + V.normalized * distanceAlongV;
                    // Draw a vector from meeting position to vectorEndPosition for the length of 'e'
                    Debug.DrawLine(meetingPosition, vectorEndPosition, Color.yellow);
                }
                print("No Collision");

            }
        }
    }

}
