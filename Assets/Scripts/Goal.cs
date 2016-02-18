using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	static public bool goalMet = false;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Projectile"){
			Goal.goalMet = true;

			Color c = renderer.material.color;
			c.a = 1;
			renderer.material.color = c;
		}
	}
}
