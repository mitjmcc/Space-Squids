using UnityEngine;
using System.Collections;

public class tentacleSegment : MonoBehaviour {

	public float len;
	public int segment;
	public float speed;
	public float timer;
	float intensity;
	public int player;
    float lerpT;
    float lerpSpeed = 0.1f;
	// Use this for initialization
	void Start () {
		timer = 0;
		intensity = 1f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		timer += speed;
		if(timer >= (Mathf.PI*2)) timer = 0;

		float rotateAngle = -intensity * Mathf.Cos (timer + segment / intensity);
        //Debug.Log(Input.GetAxis("Vertical" + player));
        if (Input.GetAxis("Pitch" + player) != 0 || Input.GetAxis("Vertical" + player) != 0)
        {
            transform.Rotate(new Vector3(Input.GetAxis("Vertical" + player) * rotateAngle, Input.GetAxis("Pitch" + player) * rotateAngle));
        }
        else
        {
            /*
            lerpT = 0;
            while (lerpT < 1)
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0, -90, 0), lerpT);
                lerpT += Time.deltaTime * lerpSpeed;
            }
            */
            Vector3 startState = Vector3.zero + transform.root.eulerAngles; 
            transform.eulerAngles = startState;
            

        }

	}
}
