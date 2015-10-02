using UnityEngine;
using System.Collections;

public class SquidShipMovement : MonoBehaviour
{
    public float playerNum;

	float turn = 0;
	float turnTarg = 0;
	float turnDrag = 0.15F;

	float roll = 0;
	float rollDrag = 0.1F;
	float rollStrength = 1/3F;

	float moveForce = 75;
	float fovBase = 65;
	float fovDrag = 1/16F;
	
	float lookSpeed = 150;
	Quaternion lookTarg = Quaternion.identity;
	float lookDrag = 0.15F;
	Quaternion shipLook = Quaternion.identity;
	float shipLookDrag = 0.1F;

	Rigidbody body;
	Camera cam;
	Transform model;
	Quaternion modelRot;

	void Start()
	{
		body = GetComponent<Rigidbody>();
		cam = transform.GetChild(0).gameObject.GetComponent<Camera>();
		model = transform.GetChild(1);
		modelRot = model.localRotation;
	}

	void FixedUpdate()
	{
		// Get movement input, strength, and direction

		var dx = Input.GetAxisRaw("Move Horizontal " + playerNum);
		var dy = Input.GetAxisRaw("Move Vertical " + playerNum);
		var len = Mathf.Sqrt(dx*dx+dy*dy);
		var dir = Mathf.Atan2(dy,dx)*Mathf.Rad2Deg+90;

        // Calculate the turn of the ship
        // The ship's turn target changes when the user is pushing hard enough in a direction, and the
        // turn angle continuously approaches its target by moving a turnDrag of their signed difference

		if (len > 0.25)
			turnTarg = dir;
		turn += Mathf.DeltaAngle(turn,turnTarg)*turnDrag;

		// Calculate the roll of the ship
		// The ship's roll target is the signed difference between its current turn and its turn target,
		// which makes the ship "bank" into directional changes with increasing strength for sharper turns
		// Like the turn angle, the roll angle continuously approaches its target with rollDrag

		var rollTarg = Mathf.DeltaAngle(turn,turnTarg)*rollStrength;
		roll += (rollTarg-roll)*rollDrag;

		// Add force in the direction the ship is facing, and expand the FOV
		// of the camera with the resultant speed (makes things look "faster")
		// The FOV also has a target and a drag to smooth it out

		var facingVec = body.rotation*Quaternion.Euler(0,turn,0)*-Vector3.right;
		body.AddForce(facingVec*len*moveForce);
		var fovTarg = fovBase+body.velocity.magnitude;
		cam.fieldOfView += (fovTarg-cam.fieldOfView)*fovDrag;
	}
	
	void Update()
	{
		// Get look input

		var dx = Input.GetAxisRaw("Look Horizontal " + playerNum);
		var dy = Input.GetAxisRaw("Look Vertical " + playerNum);
		var dz = Input.GetAxisRaw("Look Roll " + playerNum);
		dx *= dx*Mathf.Sign(dx);
		dy *= dy*Mathf.Sign(dy);
		dz *= dz*Mathf.Sign(dz);

		// Change the target look orientation based on the user's camera control input
		// and slowly interpolate this parent object's rotation to it based on the lookDrag

		var s = lookSpeed*Time.deltaTime;
		lookTarg *= Quaternion.AngleAxis(dx*s,-Vector3.up);
		lookTarg *= Quaternion.AngleAxis(dy*s,-Vector3.forward);
		lookTarg *= Quaternion.AngleAxis(dz*s,-Vector3.right);
		transform.localRotation = Quaternion.Slerp(transform.localRotation,lookTarg,lookDrag);

		// Change the ship's look orientation as well

		s = 20;

		var shipLookTarg = Quaternion.identity;
		shipLookTarg *= Quaternion.AngleAxis(dx*s,-Vector3.up);
		shipLookTarg *= Quaternion.AngleAxis(dy*s,-Vector3.forward);
		shipLookTarg *= Quaternion.AngleAxis(dz*s,-Vector3.right);
		shipLook = Quaternion.Slerp(shipLook,shipLookTarg,shipLookDrag);

		// Finally, set the position and rotation of the ship model
		// This includes a little bit of permutation for when the ship is idle

		s = Mathf.Max(0,1-body.velocity.magnitude*0.05F);

		var rotHover = Quaternion.identity;
		rotHover *= Quaternion.Euler(Mathf.Sin(Time.time*2F)*2*s,0,0);
		rotHover *= Quaternion.Euler(0,Mathf.Cos(Time.time*1.5F)*3*s,0);
		rotHover *= Quaternion.Euler(0,0,Mathf.Sin(Time.time*2.5F)*s);
		var posHover = Vector3.zero;
		posHover.x += Mathf.Cos(Time.time*1.5F)*0.03F*s;
		posHover.y += Mathf.Sin(Time.time*2.5F)*0.03F*s;
		posHover.z += Mathf.Cos(Time.time*2F)*0.015F*s;
		model.localPosition = posHover;
		
		model.rotation = body.rotation;
		model.rotation *= shipLook;
		model.rotation *= Quaternion.Euler(0,turn,0);
		model.rotation *= Quaternion.Euler(roll,0,0);
		model.rotation *= modelRot;
		model.rotation *= rotHover;
	}
}