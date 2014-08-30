using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[HideInInspector] public Ball ball = null;

	[SerializeField] private float coolDownPassTime = 0.2f;
	[SerializeField] private bool hasBall = false;
	public bool HasBall {
		get{return hasBall;}
		set{hasBall=value;}
	}

	public void Pass (Vector3 _passPosition) {
		if(!hasBall)return;
		HasBall = false;
		ball.OnKick ((_passPosition - transform.position).normalized);
		StartCoroutine (this.OnCoolDown ());

	}

	public void Shoot (Vector2 _dir)	{
		if(!hasBall)return;
		HasBall = false;
		ball.OnKick (_dir);
		StartCoroutine (this.OnCoolDown ());
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Ball") {
			HasBall = true;
			ball = c.GetComponent<Ball>();
			ball.OnPossession(transform.position);
		}
	}

	private IEnumerator OnCoolDown() {
		this.collider2D.enabled = false;
		yield return new WaitForSeconds(coolDownPassTime);
		this.collider2D.enabled = true;
	}
}
