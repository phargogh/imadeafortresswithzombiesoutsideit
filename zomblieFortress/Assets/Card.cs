using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public List<Point> walls = new List<Point>();
	public List<Point> towers = new List<Point>();
	public int cost = 5;

	// Use this for initialization
	void Start () {
		SetWalls ();
	}

	void SetWalls() {
		walls = new List<Point>(){
			new Point(-1, -1),
			new Point(-1, 0),
			new Point(-1, 1),
			new Point(0, -1),
			new Point(0, 1),
			new Point(1, -1),
			new Point(1, 0),
			new Point(1, 1),
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log ("This card was clicked");
		// deduct cost
		// 
	}
}
