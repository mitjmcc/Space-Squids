using UnityEngine;
using System.Collections;

public class squidMissile : MonoBehaviour {

    public float speed;
    public float explosionForce;
    // Use this for initialization
    public float lifeTime;
    float time;
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        time += Time.deltaTime;
        if(time > lifeTime)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position, 10);
        Destroy(gameObject);
    }
}
