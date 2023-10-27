using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    public float lifeTime = 2;
    public float damageValue = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy) {
            Destroy(gameObject, lifeTime);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (gameObject.activeInHierarchy) {
            Destroy(this.gameObject);
        }
    }
}
