using UnityEngine;
using System.Collections;

public enum TeamSide {
	SIDE_HOME, //0 is always home, blue
	SIDE_AWAY, //1 is always away, red
	COUNT
}

public class Team : MonoBehaviour {

	public TeamSide side = TeamSide.SIDE_HOME;
	public GameObject[] players;
	public bool isInPossession = false;
	[SerializeField] private int goalKeeperIndex = 0;
	public Goal teamGoal = null;

	private PlayerControl_Ball[] pc_balls;
	private PlayerControl_Movement[] pc_mvmnts;
	private int lastPlayerWithBallIndex = -1;
	private int score = 0;
	private GameObject goalKeeper;

	public int Score
	{
		get{return score;}
		set{score = value;}
	}

	public bool IsMyGoalScore
	{
		get {return teamGoal.GetScoredOnce;}
	}

	void Start () {
		pc_balls = new PlayerControl_Ball[players.Length];
		pc_mvmnts = new PlayerControl_Movement[players.Length];
		for( int i=0; i<players.Length; ++i ) {
			pc_balls[i] = players[i].GetComponent<PlayerControl_Ball>();
			pc_mvmnts[i] = players[i].GetComponent<PlayerControl_Movement>();
		}
		goalKeeper = players [goalKeeperIndex];
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

	public void AddScore()  {
		score += 1;
	}

	public void GiveBallToGoalKeeper(GameObject _ball) {
		EnableAllPlayers ();
		_ball.rigidbody2D.velocity *= 0;
		_ball.transform.position = goalKeeper.transform.position;
	}
}
