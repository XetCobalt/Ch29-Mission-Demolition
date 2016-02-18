using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
	static public Slingshot S;
	//fields set in the Unity Inspector pane
	public GameObject prefabProjectile;
	public float velocityMult = 10f;
	public bool ___________________;
	//fields set dynamically
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;

	void Awake(){
		S = this;
		Transform launchPointTrans = transform.Find ("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive (false);
		launchPos = launchPointTrans.position;
	}

	void OnMouseEnter(){
		//print ("Slingshot:OnMouseEnter()");
		launchPoint.SetActive (true);
	}

	void OnMouseExit(){
		//print ("Slingshot:OnMouseExit()");
		launchPoint.SetActive (false);
	}

	void OnMouseDown(){
		//the player has pressed the mouse button while over slingshot
		aimingMode = true;
		//instantiate a projectile
		projectile = Instantiate (prefabProjectile) as GameObject;
		//start it at launchPoint
		projectile.transform.position = launchPos;
		//set it to isKinematic for now
		projectile.rigidbody.isKinematic = true;
	}

	void Update(){
		//if sling isn't in aiming, don't run this code
		if (!aimingMode) {
			return;
		}
		//get current mouse position in 2D coord.
		Vector3 mousePos2D = Input.mousePosition;
		//convert mouse position to 3D coord.
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);
		//find the delta
		Vector3 mouseDelta = mousePos3D - launchPos;
		//limit mousedelta to spherecollider radius of slingshot
		float maxMagnitude = this.GetComponent<SphereCollider> ().radius;
		if(mouseDelta.magnitude > maxMagnitude){
			mouseDelta.Normalize ();
			mouseDelta *= maxMagnitude;
		}
		//move projectile to new pos
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		if (Input.GetMouseButtonUp (0)) {
			//the mouse has been released
			aimingMode = false;
			projectile.rigidbody.isKinematic = false;
			projectile.rigidbody.velocity = -mouseDelta * velocityMult;
			FollowCam.S.poi = projectile;
			projectile = null;
			MissionDemolition.ShotFired ();
		}
	}
}
