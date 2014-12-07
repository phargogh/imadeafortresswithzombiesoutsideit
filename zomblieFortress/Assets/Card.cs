using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {


	private static System.Random rand = new System.Random();

	public List<Point> walls = new List<Point>();
	public List<Point> towers = new List<Point>();
	public int cost = 5;
	private Board board;
	private Hand hand;
	public bool isSelected = false;

	public Color defaultColor = new Color32(139,138,127,255);
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
		int num_walls = rand.Next (3, 8);
		walls = CardGen.createShape (num_walls);
	}

	void SetTowers (){
		int num_towers = rand.Next (0,1);
		for (int i = 0; i < num_towers; i++){
			this.towers.Add(CardGen.ChoosePoint(this.walls));
		};
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
		if (isSelected) {
			hand.Unselect();
		} else {
			hand.SelectCard(this);
		}
	}
}
