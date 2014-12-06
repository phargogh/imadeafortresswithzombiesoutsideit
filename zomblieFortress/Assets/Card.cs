using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public List<Point> walls = new List<Point>();
	public List<Point> towers = new List<Point>();
	public int cost = 5;
	private Board board;
	private Hand hand;

	// Use this for initialization
	void Start () {

	}

	public void init (Board board, Hand hand) {
		Debug.Log ("Card init");
		this.board = board;
		this.hand = hand;
		SetWalls ();
	}

	void SetWalls() {
		walls = new List<Point>(){
			//new Point(-1, -1),
			//new Point(-1, 0),
			//new Point(-1, 1),
			//new Point(0, -1),
			//new Point(0, 1),
			//new Point(1, -1),
			//new Point(1, 0),
			//new Point(1, 1),
			new Point(12, 12),
			new Point(11, 13),
			new Point(10, 14)
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log ("This card was clicked");
		hand.playCard (this);
	}
}
