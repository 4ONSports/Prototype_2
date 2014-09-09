using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	private GameObject ball = null;
	private Team myTeam;

	public GameObject GetBall {
		get	{ return ball;}
	}

	void Start() {
		this.collider2D.isTrigger = true;
	}

	public void AssignTeam(Team _myTeam) {
		myTeam = _myTeam;
	}

	void OnTriggerEnter2D (Collider2D c) {
		if (c.tag == "Ball") {
			ball = c.gameObject;
			GameEvents_2.BroadcastGoalScored(myTeam);
		}
	}
}
