using UnityEngine;
using System.Collections;

public class SquidDriver : MonoBehaviour {

	public int playerNum;

	public float forwardForce;
	public float boostForce;

	public Transform InkPulser;
	
	public GameObject[] checkpoints;
	public int currentCheckpoint;
	public Transform pointer;

	public int powerup;

	// Use this for initialization
	void Start () 
	{
		//currentCheckpoint = 0;
		powerup = 0;
		Debug.Log("Current Checkpoint: " + currentCheckpoint);
	}
	
	// Update is called once per frame
	void Update () 
	{
		nextCheckpointPointer ();

		controls ();
	}

	//overarching contol
	void controls()
	{

		transform.Rotate(-1 * Vector3.left * Input.GetAxis ("Vertical" + playerNum));
		transform.Rotate(-1 * Vector3.forward * Input.GetAxis ("Horizontal" + playerNum));
		transform.Rotate(-1 * Vector3.up * Input.GetAxis ("Pitch" + playerNum));
		if (Input.GetButton ("Forward"  + playerNum))
		{
			GetComponent<Rigidbody>().AddForce(transform.forward * forwardForce);
		}

		if (Input.GetButtonDown("Fire"+  + playerNum)) 
		{
			if ( powerup == 1)
			{
				Instantiate(InkPulser,transform.position + new Vector3(0,0,0),Quaternion.identity);
				powerup = 0;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Checkpoint") 
		{
			//Debug.Log("Checkpoint!");
			if(other.gameObject.transform.parent.gameObject.GetInstanceID() == checkpoints[currentCheckpoint].GetInstanceID())
			//if(other.gameObject.transform.parent.position == checkpoints[currentCheckpoint].GetInstanceID)
			{
				currentCheckpoint = (currentCheckpoint + 1) % checkpoints.Length;
				Debug.Log("Current Checkpoint: " + currentCheckpoint);
			}
		}

		if (other.tag == "Boost") 
		{
			GetComponent<Rigidbody>().AddForce(transform.forward * boostForce);
		}

		if (other.tag == "Powerup") 
		{
			if (powerup == 0)
			{
				powerup = 1;
				Destroy(other.gameObject);
				Debug.Log("Current Powerup: " + powerup);
			}
		}
	}

	void nextCheckpointPointer()
	{
		//pointer.LookAt(checkpoints[currentCheckpoint].transform.position);
		//Debug.Log("Current Checkpoint: " + currentCheckpoint);
	}
}
	