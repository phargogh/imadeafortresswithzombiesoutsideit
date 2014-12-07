using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public List<Point> walls = new List<Point>();
	public List<Point> towers = new List<Point>();
	public int cost = 5;
	private Board board;
	private Hand hand;
	public bool isSelected = false;

	public Color defaultColor = Color.gray;
	public Color selectedColor = Color.yellow;

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
		walls = CardGen.createShape (4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void select() {
		isSelected = true;
		GetComponent<SpriteRenderer>().color = selectedColor;
	}

	public void unselect() {
		isSelected = false;
		GetComponent<SpriteRenderer>().color = defaultColor;
	}

	void OnMouseDown(){
		Debug.Log ("This card was clicked");
		hand.SelectCard(this);
	}
}
