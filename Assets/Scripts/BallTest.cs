using UnityEngine;
using System.Collections;

public class BallTest : MonoBehaviour {
	
	void Start () {
		this.rigidbody2D.AddForce (new Vector2(Random.value,Random.value)*500);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
