using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[System.Serializable]
public class MovementParameters {
	[Header("Range")]
	[Range(0, 15)]
	public int xRange;
	[Range(0, 15)]
	public int yRange;
	[Range(0, 15)]
	public int zRange;
	public float acceleration;
	public int topSpeed;

	public float turnSpeed;
	[Range(0, 1)]
	public double turnChance;
}

[RequireComponent(typeof(Rigidbody))]
public class DroneMovement : MonoBehaviour {
	public MovementParameters parameters;

	public GameObject target;

	private Rigidbody rb;
	private bool attackMode;

	private Vector3 heading;
	private Vector3 randomHeading;
	private Vector3 start;
	private System.Random r;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		attackMode = true;
		randomHeading = transform.forward;
		start = transform.position;

		r = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
		if (attackMode) {
			if ((parameters.turnChance / 60) > r.NextDouble()) {
				randomHeading = new Vector3((float)r.NextDouble()*2-1, (float)r.NextDouble()*2-1, (float)r.NextDouble()*2-1);
				Debug.Log(randomHeading);
			}

			Vector3 offset = start - transform.position;

			// make sure the drone stays in bounds
			if (Math.Abs(offset.x) >= parameters.xRange) {
				randomHeading.x = (randomHeading.x + (offset.x / Math.Abs(offset.x))) * 0.2f;
				Debug.Log(randomHeading);
			}
			if (Math.Abs(offset.y) >= parameters.yRange) {
				randomHeading.y = (randomHeading.y + (offset.y / Math.Abs(offset.y))) * 0.2f;
				Debug.Log(randomHeading);
			}
			if (Math.Abs(offset.z) >= parameters.zRange) {
				randomHeading.z = (randomHeading.z + (offset.z / Math.Abs(offset.z)))* 0.2f;
				Debug.Log(randomHeading);
			}

			rb.AddForce(randomHeading * parameters.acceleration);

			Vector3 stepTurn = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, parameters.turnSpeed*Time.deltaTime, 0.0f);			transform.rotation = Quaternion.LookRotation(stepTurn);
			transform.rotation = Quaternion.LookRotation(stepTurn);

			if (rb.velocity.magnitude > parameters.topSpeed) {
				rb.velocity = (parameters.topSpeed / rb.velocity.magnitude) * rb.velocity;
			}
		}

		else {
			if ((parameters.turnChance / 60) > r.NextDouble()) {
				randomHeading = new Vector3((float)r.NextDouble()*2-1, (float)r.NextDouble()*2-1, (float)r.NextDouble()*2-1);
				Debug.Log(randomHeading);
			}

			Vector3 offset = start - transform.position;

			// make sure the drone stays in bounds
			if (Math.Abs(offset.x) >= parameters.xRange) {
				randomHeading.x = (randomHeading.x + (offset.x / Math.Abs(offset.x))) * 0.2f;
				Debug.Log(randomHeading);
			}
			if (Math.Abs(offset.y) >= parameters.yRange) {
				randomHeading.y = (randomHeading.y + (offset.y / Math.Abs(offset.y))) * 0.2f;
				Debug.Log(randomHeading);
			}
			if (Math.Abs(offset.z) >= parameters.zRange) {
				randomHeading.z = (randomHeading.z + (offset.z / Math.Abs(offset.z)))* 0.2f;
				Debug.Log(randomHeading);
			}

			rb.AddForce(randomHeading * parameters.acceleration);

			Vector3 stepTurn = Vector3.RotateTowards(transform.forward, randomHeading, parameters.turnSpeed*Time.deltaTime, 0.0f);
			stepTurn.y = 0;
			transform.rotation = Quaternion.LookRotation(stepTurn);

			if (rb.velocity.magnitude > parameters.topSpeed) {
				rb.velocity = (parameters.topSpeed / rb.velocity.magnitude) * rb.velocity;
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		randomHeading *= -1;
	}
}
