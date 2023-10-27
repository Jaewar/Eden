using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetector : MonoBehaviour
{

    public static RayCastDetector instance;

    public float range = 5;

    private Ray theRay;


    private void Start() {
        instance = this;
        theRay = new Ray();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward;
        theRay = new Ray(transform.position, transform.TransformDirection(direction * range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

      /*  if (Physics.Raycast(theRay, out RaycastHit hit, range)) {
            
        }*/
    }

    public Ray getRay() {
        return theRay;
    }
}
