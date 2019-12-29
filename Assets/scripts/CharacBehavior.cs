using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacBehavior : MonoBehaviour {

	public Rigidbody2D rb;
	public float vitesse;
	public float maxJump;

	public float speed = 10f;
	private int directionMove = 0;

	// Use this for initialization
	void Start () {
		rb.velocity += new Vector2(vitesse,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			Jump();
		}
		
		if(Input.GetKeyDown("right")){
			MoveRight();
		}

		if(Input.GetKeyDown("left")){
			MoveLeft();
		}

	}

	void Jump() {
		rb.velocity += new Vector2(0, maxJump);
	}

	void MoveRight(){
		rb.velocity += new Vector2(speed, 0);
	}

	void MoveLeft(){
		rb.velocity += new Vector2(-speed, 0);
	}

}
