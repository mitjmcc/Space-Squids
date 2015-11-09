using UnityEngine;
using System.Collections;

public class Orbiter : MonoBehaviour {

    public Transform center;
    public Vector3 axis;
    public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(center.position, axis, speed);
	}
}
