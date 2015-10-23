using UnityEngine;
using System.Collections;

public class JohnCenaMovement : MonoBehaviour {
    public float playerNum;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Move Vertical " + playerNum) == -1)
        {
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
