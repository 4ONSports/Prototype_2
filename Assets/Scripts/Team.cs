using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {
	public enum TeamSide {
		SIDE_HOME,
		SIDE_AWAY,
		COUNT
	}
	public GameObject[] players;
	public bool isInPossession = false;
	
	private PlayerControl_Ball[] pc_balls;
	private PlayerControl_Movement[] pc_mvmnts;
	private int lastPlayerWithBallIndex = -1;

	// Use this for initialization
	void Start () {
		pc_balls = new PlayerControl_Ball[players.Length];
		pc_mvmnts = new PlayerControl_Movement[players.Length];
		for( int i=0; i<players.Length; ++i ) {
			pc_balls[i] = players[i].GetComponent<PlayerControl_Ball>();
			pc_mvmnts[i] = players[i].GetComponent<PlayerControl_Movement>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void EnableAllPlayers () {
		for( int i=0; i<players.Length; ++i ) {
			pc_balls[i].disable = false;
			pc_mvmnts[i].disable = false;

			Color col = pc_mvmnts[i].renderer.material.color;
			col.a = (float)126/255;
			pc_mvmnts[i].renderer.material.color = col;
		}
	}
	
	public void DisableLastPlayerWithBall () {
		if( lastPlayerWithBallIndex >= 0 ) {
			pc_balls[lastPlayerWithBallIndex].disable = true;
			pc_mvmnts[lastPlayerWithBallIndex].disable = true;

			Color col = pc_mvmnts[lastPlayerWithBallIndex].renderer.material.color;
			col.a = (float)20/255;
			pc_mvmnts[lastPlayerWithBallIndex].renderer.material.color = col;
		}
	}
	
	public void UpdatePossession () {
		isInPossession = false;
		for( int i=0; i<players.Length; ++i ) {
			if( pc_balls[i].hasABall ) {
				isInPossession = true;
				lastPlayerWithBallIndex = i;
			}
		}

		if( isInPossession ) {
			for( int i=0; i<players.Length; ++i ) {
				pc_mvmnts[i].disable = true;
			}
		}
	}
}
