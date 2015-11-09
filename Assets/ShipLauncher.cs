using UnityEngine;
using System.Collections;

public class ShipLauncher : MonoBehaviour {
    public GameObject ship;
    // Use this for initialization
    float t;
    public float interval;
	void Start () {
        t = 0;
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;

        if(t > interval)
        {
            t = 0;
            Vector3 randomSpawn = new Vector3(Random.Range(-600, 300), Random.Range(-150, 650), Random.Range(-1950, -1400));
            Quaternion randomRotation = Random.rotation;
            Instantiate(ship,randomSpawn, randomRotation);
        }
    }
}
