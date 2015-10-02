using UnityEngine;
using System.Collections;

public class OrbitalMovement : MonoBehaviour {

    public float radius, speed, delta, xoffset = 1, zoffset = 1, xtest, ztest;
    Vector3 newPos;

	void Start () {
        newPos = new Vector3(0, 0, 0);
	}
	
	void FixedUpdate () {
        delta = Time.fixedTime;
        newPos.Set(xoffset * radius * Mathf.Cos(speed * delta ),
            radius * Mathf.Sin(speed * delta + xtest),
            zoffset * radius * Mathf.Sin(speed * delta));
        transform.position = newPos;
	}
}
