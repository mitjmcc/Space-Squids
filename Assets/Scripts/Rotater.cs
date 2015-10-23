using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {

    public float RotateXSpeed;
    public float RotateYSpeed;
    public float RotateZSpeed;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(RotateXSpeed, RotateYSpeed, RotateZSpeed));
	}
}
