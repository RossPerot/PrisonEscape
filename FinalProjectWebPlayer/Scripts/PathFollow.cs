using UnityEngine;
using System.Collections;

public class PathFollow : Vehicle {
	public float inFront;
	public float future;
	public float seekWt;
	public float fleeWt;
	public float avoidWt;
	public float containWt;
	public float width;
	public float fleeDistance;
	public float avoidDist;
	public float escapePath;
    public bool isActive = true;
	GameObject[] path;
	GameObject[] guards;
	Vector3 target;
	Vector3 futurePosition;
	Vector3 closestPoint;
	float closestDistance;
	float closestObstacleDistance;
	GameObject wp;
	GameObject closestGuard;
	GameObject[] obstacles;

	// Use this for initialization
	void Start () {
		base.Start (); 
		if (escapePath == 0) {
			path = GameObject.FindGameObjectsWithTag ("WP");
		}
		if (escapePath == 1) {
			path = GameObject.FindGameObjectsWithTag ("WP1");
		}
		guards = GameObject.FindGameObjectsWithTag ("Guard");
		obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");
		getClosestPoint ();
		getClosestGuard ();
	}
	
	protected override void CalcSteeringForce(){

         Vector3 force = Vector3.zero;
         if (isActive)
            {
            nextClosestPoint();
            target = closestPoint + wp.GetComponent<WayPoint>().unitVec * inFront;
            if (closestDistance > width / 2)
            {
                force += seekWt * Seek(target);
            }
            getClosestGuard();
            if (closestDistance < 2)
            {
                if (escapePath == 0)
                {
                    transform.position = new Vector3(60, 1.15f, -80);
                    getClosestPoint();
                }
                else if (escapePath == 1)
                {
                    transform.position = new Vector3(80, 1.15f, -77);
                    getClosestPoint();
                }
            }
            obstacleCollision();
            if (closestObstacleDistance < 8)
            {
                if (escapePath == 0)
                {
                    transform.position = new Vector3(60, 1.15f, -80);
                    getClosestPoint();
                }
                else if (escapePath == 1)
                {
                    transform.position = new Vector3(80, 1.15f, -77);
                    getClosestPoint();
                }
            }
            if (Vector3.Magnitude(transform.position - closestGuard.transform.position) < fleeDistance)
            {
                force += fleeWt * AvoidObstacle(closestGuard, avoidDist);
            }
            else if (transform.position.x > 90)
            {
                force += containWt * ((Vector3.left * maxSpeed) - velocity);
            }
            else if (transform.position.x < -100)
            {
                force += containWt * ((Vector3.right * maxSpeed) - velocity);
            }
            else if (transform.position.z > 100)
            {
                force += containWt * ((Vector3.back * maxSpeed) - velocity);
            }
            for (int i = 0; i < obstacles.Length; i++)
            {
                force += avoidWt * AvoidObstacle(obstacles[i], avoidDist);
            }
        }
        if (transform.position.z < -100)
        {
            isActive = false;
            velocity = Vector3.zero;
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
			if(curClosestDistance < closestDistance){
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
		for (int i=0; i < path.Length; i++) {
			GameObject curWP = path[i];
			curClosestPoint = curWP.GetComponent<WayPoint>().closestPoint(futurePosition);
			curClosestDistance = Vector3.Distance(curClosestPoint, futurePosition);
			if(curClosestDistance < closestDistance){
				closestDistance = curClosestDistance;
				closestPoint = curClosestPoint;
				wp = curWP;
			}
		}
	}
	private void getClosestGuard(){
		closestDistance = 1000;
		float currentDistance;
		GameObject currentGuard;
		for (int i=0; i < guards.Length; i++) {
			currentGuard = guards[i];
			currentDistance = Vector3.Magnitude (currentGuard.transform.position - this.transform.position);
			if (currentDistance < closestDistance){
				closestDistance = currentDistance;
				closestGuard = currentGuard;
			}
		}
	}
	private void obstacleCollision(){
		closestObstacleDistance = 1000;
		float currentDistance;
		GameObject currentObstacle;
		for (int i=0; i < obstacles.Length; i++) {
			currentObstacle = obstacles [i];
			currentDistance = Vector3.Magnitude (currentObstacle.transform.position - this.transform.position);
			if (currentDistance < closestDistance) {
				closestObstacleDistance = currentDistance;
			}
		}
	}

}
