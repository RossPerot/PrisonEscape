using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour {
	
	GameObject pathGuy; // a PathFollower
	public float numberOfPrisoners;
	public int numOfObst;
	//prefabs
	public GameObject PathFollowerPrefab;
	public Object obstaclePrefab;

	private  GameObject[] obstacles;
	public GameObject[] Obstacles {get{return obstacles;}}

	private List<GameObject> prisoners = new List<GameObject>();
	public List<GameObject> Prisoners {get{return prisoners;}}

	// These weights will be exposed in the Inspector window
	public float seekWt = 75.0f;

    public Camera camera;
    public Camera[] cameras;
    public int camCount = 0;

	void Start () {
        camera = Camera.main;
	}
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            camCount++;
            if (camCount > cameras.Length - 1)
            {
                camCount = 0;
            }
        }
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }
        cameras[camCount].camera.enabled = true;
    }
}
