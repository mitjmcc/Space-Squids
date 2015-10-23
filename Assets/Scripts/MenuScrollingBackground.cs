using UnityEngine;
using System.Collections;

public class MenuScrollingBackground : MonoBehaviour {

    public int x;
    public int y;
    int len = 621;
    int wid = 297;
    float xSpeed = 650f/743f;
    float ySpeed = 360f / 743f;
    public float speed;
    public int end;
    Vector3 moveSpeed;
    // Use this for initialization
    void Start () {
        moveSpeed = new Vector3(xSpeed, ySpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - moveSpeed.x,transform.position.y - moveSpeed.y);
        if (transform.position.x < -325)
        {
            transform.position = new Vector3(len*4,wid*4,0);
        }
	}
}
