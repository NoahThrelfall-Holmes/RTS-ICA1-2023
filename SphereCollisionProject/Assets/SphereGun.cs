using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereGun : MonoBehaviour
{
    [SerializeField] CustomSphere sphere;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CustomSphere newSphere = Instantiate(sphere) as CustomSphere;
            newSphere.transform.position = transform.position;
            newSphere.SetMass(Random.Range(0.1f, 2.0f));
            newSphere.transform.localScale = Vector3.one * newSphere.GetMass();
            newSphere.velocity = (Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(1f) - transform.position) * Random.Range(2f, 10f);
        }
    }
}
