using UnityEngine;
using System.Collections;

public class tentacleSegment : MonoBehaviour {

	public float len;
	public int segment;
	public float speed;
	public float timer;
	float intensity;
	// Use this for initialization
	void Start () {
		timer = 0;
		intensity = 0.75f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		timer += speed;
		if(timer >= (Mathf.PI*2)) timer = 0;

		transform.Rotate (new Vector3 (0,0,-intensity * Mathf.Cos (timer + segment/intensity)));
	}
}
