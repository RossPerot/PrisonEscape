using UnityEngine;
using System.Collections;

public class Wanderer : Vehicle {

	public float inFront;
	public float future;
	public float wanderWt;
	public float width;
	
	// Use this for initialization
	void Start () {
		base.Start (); 
	}
	
	protected override void CalcSteeringForce(){
		Vector3 force = Vector3.zero;
		force += wanderWt * Wander ();
		//limit force to maxForce and apply
		force = Vector3.ClampMagnitude (force, maxForce);
		ApplyForce(force);
		
	}
}
