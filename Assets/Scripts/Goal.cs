using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	private bool scoredOnMe = false;
	private GameObject ball = null;

	//TODO: part of reactoring,  remove this get
	public bool GetScoredOnce {
		get
		{ 	
			bool b = scoredOnMe;
			scoredOnMe = false;
			return b;
		}
	}

	public GameObject GetBall {
		get	{ return ball;}
	}

	void Start() {
		this.collider2D.isTrigger = true;
	}

	void OnTriggerEnter2D (Collider2D c) {
		if (c.tag == "Ball") {
			ball = c.gameObject;
			GameEvents.TriggerEvent(GameEvents.GameEvent.EVT_GOAL_SCORED);
			scoredOnMe= true;
		}
	}
}
