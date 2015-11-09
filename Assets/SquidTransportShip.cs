using UnityEngine;
using System.Collections;

public class SquidTransportShip : MonoBehaviour {

    // Use this for initialization
    public float length;
    public float speed;
	void Start () {
        //GetComponent<ParticleSystem>().Emit(100);
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Destroy(gameObject, length);
	}
}
