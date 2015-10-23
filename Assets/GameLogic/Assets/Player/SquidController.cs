using UnityEngine;
using System.Collections;

public class SquidController : MonoBehaviour
{
	public int playerIndex = 0;
	float control = 0;
	float rumble = 0;

	float turn = 0;
	float turnTarg = 0;
	float turnDrag = 0.15F;

	float roll = 0;
	float rollDrag = 0.1F;
	float rollStrength = 1/4F;

	float moveForce = 75;
	float moveBoost = 1;
	float moveBoostMax = 3F;
	float moveBoostDrag = 1/32F;
	float moveBoostRoll = 0;
	float fovBase = 65;
	float fovDrag = 1/8F;
	
	float lookSpeed = 150;
	Quaternion lookTarg = Quaternion.identity;
	float lookDrag = 0.15F;
	Quaternion shipLook = Quaternion.identity;
	float shipLookSpeed = 20F;
	float shipLookDrag = 0.1F;

	Rigidbody body;
	Camera cam;
	Transform model;
	Quaternion modelRot;
	ParticleSystem boostFX;
	TrailRenderer trailFX;
	AudioSource engineSound;

	Vector3 camPos;
	Quaternion camRot;
	Vector3 camTargPos = new Vector3(3,1,0);
	Quaternion camTargRot = Quaternion.Euler(0,270,0);
	float camDrag = 1.1F;

	void Awake()
	{
		body = GetComponent<Rigidbody>();
		cam = transform.Find("Camera").gameObject.GetComponent<Camera>();
		model = transform.Find("Model");
		modelRot = model.localRotation;
		boostFX = transform.Find("Model/BoostFX").gameObject.GetComponent<ParticleSystem>();
		trailFX = transform.Find("Model/Trail").gameObject.GetComponent<TrailRenderer>();
		engineSound = GetComponent<AudioSource>();
	}

	void Start()
	{
		camPos = new Vector3(100,40,(playerIndex-0.5F)*50F);
		camRot = Quaternion.LookRotation(new Vector3(0,1,0)-camPos);
		cam.transform.localPosition = camPos;
		cam.transform.localRotation = camRot;
	}

	void FixedUpdate()
	{
		// Get movement input, strength, and direction

		var dx = Input.GetAxis("Move Horizontal "+playerIndex)*control;
		var dy = Input.GetAxis("Move Vertical "+playerIndex)*control;
		var len = Mathf.Min(Mathf.Sqrt(dx*dx+dy*dy),1F);
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
		moveBoost += (1-moveBoost)*moveBoostDrag;
		moveBoostRoll = (moveBoost-1)*(moveBoost-1)*-90;
		body.AddForce(facingVec*len*moveForce*moveBoost);

		var fovTarg = fovBase+body.velocity.magnitude*(4F+moveBoost)/5F-rumble*30;
		cam.fieldOfView += (fovTarg-cam.fieldOfView)*fovDrag;

		// Make the trail widen with the boost, make the boost particle fade,
		// and make the pitch of the engine sound relate to the speed

		trailFX.startWidth = moveBoost-0.5F;
		float s = Mathf.Max(0,moveBoost-1.5F);
		boostFX.startColor = new Color(1F,1F,1F,1-(1-s)*(1-s));
		engineSound.pitch = 1+body.velocity.magnitude*0.4F;
	}

	public void setControl(int input)
	{
		control = input;
	}

	public void boost()
	{
		boostFX.Play();
		moveBoost = moveBoostMax;
	}

	public void setRumble(float input)
	{
		rumble = input;
	}

	void Update()
	{
		// Get look input

		var dx = Input.GetAxis("Look Horizontal "+playerIndex)*control;
		var dy = Input.GetAxis("Look Vertical "+playerIndex)*control;
		var dz = Input.GetAxis("Look Roll "+playerIndex)*control;
		dx *= dx*Mathf.Sign(dx);
		dy *= dy*Mathf.Sign(dy);
		dz *= dz*Mathf.Sign(dz);

		// Change the target look orientation based on the user's camera control input
		// and slowly interpolate this parent object's rotation to it based on the lookDrag

		var s = lookSpeed*Time.deltaTime*(2F+moveBoost)/3F;
		lookTarg *= Quaternion.AngleAxis(dx*s,-Vector3.up);
		lookTarg *= Quaternion.AngleAxis(dy*s,-Vector3.forward);
		lookTarg *= Quaternion.AngleAxis(dz*s,-Vector3.right);
		transform.localRotation = Quaternion.Slerp(transform.localRotation,lookTarg,lookDrag);

		// Change the ship's look orientation as well

		s = shipLookSpeed;

		var shipLookTarg = Quaternion.identity;
		shipLookTarg *= Quaternion.AngleAxis(dx*s,-Vector3.up);
		shipLookTarg *= Quaternion.AngleAxis(dy*s,-Vector3.forward);
		shipLookTarg *= Quaternion.AngleAxis(dz*s,-Vector3.right);
		shipLook = Quaternion.Slerp(shipLook,shipLookTarg,shipLookDrag);

		// Finally, set the position and rotation of the ship model
		// This includes a little bit of permutation for when the ship is idle

		s = Mathf.Max(0,1-body.velocity.magnitude*0.05F);

		float hOff = playerIndex*5F;
		var rotHover = Quaternion.identity;
		rotHover *= Quaternion.Euler(Mathf.Sin(Time.time*2F+hOff)*2*s,0,0);
		rotHover *= Quaternion.Euler(0,Mathf.Cos(Time.time*1.5F+hOff)*3*s,0);
		rotHover *= Quaternion.Euler(0,0,Mathf.Sin(Time.time*2.5F+hOff)*s);
		var posHover = Vector3.zero;
		posHover.x += Mathf.Cos(Time.time*1.5F+hOff)*0.03F*s;
		posHover.y += Mathf.Sin(Time.time*2.5F+hOff)*0.03F*s;
		posHover.z += Mathf.Cos(Time.time*2F+hOff)*0.015F*s;
		model.localPosition = posHover;
		
		model.rotation = body.rotation;
		model.rotation *= shipLook;
		model.rotation *= Quaternion.Euler(0,turn,0);
		model.rotation *= Quaternion.Euler(roll+moveBoostRoll,0,0);
		model.rotation *= modelRot;
		model.rotation *= rotHover;

		// Slowly center the camera over time

		camPos = Vector3.Lerp(camPos,camTargPos,camDrag*Time.deltaTime);
		camRot = Quaternion.Slerp(camRot,camTargRot,camDrag*Time.deltaTime);
		cam.transform.localPosition = camPos + new Vector3(rumble*2F,-rumble*0.25F,0);
		Vector3 jitter = Random.insideUnitSphere;
		jitter.Scale(new Vector3(rumble*0.075F,rumble*0.075F,rumble*0.075F));
		cam.transform.localPosition += jitter;
		cam.transform.localRotation = camRot;
	}
}