using UnityEngine;
using System.Collections;

public class Patrol : Vehicle {
	public float inFront;
	public float future;
	public float seekWt;
	public float chaseWt;
    public float wanderWt;
	public float width;
	public float chaseDistance;
	public float patrolPath;
	public float obstacleWidth;
	GameObject[] guardPath;
	Vector3 target;
	Vector3 futurePosition;
	Vector3 closestPoint;
	float closestDistance;
	GameObject wp;
	GameObject prisoner;
	
	// Use this for initialization
	void Start () {
		base.Start (); 
		prisoner = GameObject.FindGameObjectWithTag("Prisoner");
		if (patrolPath == 1){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP1");
		}
		if (patrolPath == 2){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP2");
		}
		if (patrolPath == 3){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP3");
		}
		if (patrolPath == 4){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP4");
		}
		if (patrolPath == 5){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP5");
		}
		if (patrolPath == 6){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP6");
		}
		if (patrolPath == 7){
			guardPath = GameObject.FindGameObjectsWithTag ("GuardWP7");
		}
		getClosestPoint ();
	}
	
	protected override void CalcSteeringForce(){
		Vector3 force = Vector3.zero;
		nextClosestPoint ();
		target = closestPoint + wp.GetComponent<WayPoint> ().unitVec * inFront;
		if (closestDistance > width / 2) {
			force += seekWt * Seek (target);
		}
		if (Vector3.Magnitude(transform.position - prisoner.transform.position) < chaseDistance) {
			force += chaseWt * Seek (prisoner.transform.position);
		}
		//limit force to maxForce and apply
		force = Vector3.ClampMagnitude (force, maxForce);
		ApplyForce(force);
		
		//show force as a blue line pushing the guy like a jet stream
		Debug.DrawLine(transform.position, transform.position - force,Color.blue);
		//red line to the target which may be out of sight
		Debug.DrawLine (transform.position, target,Color.red);
		
	}
	
	private void nextClosestPoint(){
		GameObject[] nextPoints = new GameObject[2];
		nextPoints[0] = wp.gameObject;
		nextPoints [1] = nextPoints[0].GetComponent<WayPoint>().next;
		closestDistance = 1000;
		float curClosestDistance;
		Vector3 curClosestPoint;
		futurePosition = transform.position + velocity * future;
		for (int i=0; i < 2; i++) {
			GameObject curWP = nextPoints[i];
			curClosestPoint = curWP.GetComponent<WayPoint>().closestPoint(futurePosition);
			curClosestDistance = Vector3.Distance(curClosestPoint, futurePosition);
			if(curClosestDistance < closestDistance || wp.GetComponent<WayPoint>().endpoint == true){
				closestDistance = curClosestDistance;
				closestPoint = curClosestPoint;
				wp = curWP;
			}
		}
	}
	
	private void getClosestPoint(){
		closestDistance = 1000;
		float curClosestDistance;
		Vector3 curClosestPoint;
		futurePosition = transform.position + velocity * future;
		for (int i=0; i < guardPath.Length; i++) {
			GameObject curWP = guardPath[i];
			curClosestPoint = curWP.GetComponent<WayPoint>().closestPoint(futurePosition);
			curClosestDistance = Vector3.Distance(curClosestPoint, futurePosition);
			if(curClosestDistance < closestDistance){
				closestDistance = curClosestDistance;
				closestPoint = curClosestPoint;
				wp = curWP;
			}
		}
	}
}