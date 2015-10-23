using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckpointSystem : MonoBehaviour {

    public Transform checkpoints;
    public Transform[] checkpointsList;
    public int currentCheckpoint;
    public int currentLap;

    public Text lapCounter;
    public Text checkpointCounter;

    // Use this for initialization
    void Start () {
        Debug.Log(checkpoints.childCount);
        checkpointsList = new Transform[checkpoints.childCount];
        for(int i = 0; i < checkpointsList.Length; i++)
        {
            checkpointsList[i] = checkpoints.GetChild(i);
        }
        currentCheckpoint = 0;
        currentLap = 0;
        lapCounter.text = "Lap: " + currentLap + "/3";
        checkpointCounter.text = "Checkpoint: " + currentCheckpoint + "/" + checkpointsList.Length;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            if (other.transform.parent.GetInstanceID() == checkpointsList[currentCheckpoint].GetInstanceID())
            {
                if(currentCheckpoint == 0)
                {
                    currentLap++;
                    lapCounter.text = "Lap: " + currentLap + "/3";
                }
                

                currentCheckpoint = (currentCheckpoint + 1) % checkpointsList.Length;
                checkpointCounter.text = "Checkpoint: " + currentCheckpoint + "/" + checkpointsList.Length;


            }
        }
    }
}
