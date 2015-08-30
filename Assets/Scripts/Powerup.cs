using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public Transform sphere1;
	public Transform sphere2;
	public Transform sphere3;

	public float angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		sphere1.Rotate(new Vector3(0,1,0),angle);
		sphere2.Rotate(new Vector3(0,1,0),angle);
		sphere3.Rotate(new Vector3(0,0,1),angle);
	}
}
