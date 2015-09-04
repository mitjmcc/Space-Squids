using UnityEngine;
using System.Collections;

public class checkpointRotater : MonoBehaviour {

	// Use this for initialization
	public float speed;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		transform.Rotate(new Vector3(0,0,speed));
	}
}
