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

	public Color defaultColor; 
	public Color selectedColor; 

	public Stack<GameObject> cardSquares = new Stack<GameObject> ();
	private float cardScale = 0.5f;

	// Use this for initialization
	void Start () {
		defaultColor = Palette.DarkBrown;
		selectedColor = Palette.Red;
	}

	public void init (Board board, Hand hand) {
		Debug.Log ("Card init");
		this.board = board;
		this.hand = hand;
		SetWalls ();
		SetTowers ();
		//RefreshDisplay ();
	}

	void RefreshDisplay ()
	{
		foreach(GameObject g in cardSquares) {
			g.SetActive(false);
		}
		cardSquares.Clear ();
		foreach(Point w in walls) {
			Vector3 pos = GridOffsetToCardPos(w);
			GameObject g = (GameObject) Instantiate(board.wallFab, pos, Quaternion.identity);
			g.transform.localScale = g.transform.localScale * cardScale;
			g.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	Vector3 GridOffsetToCardPos(Point gridOffest) {
		float x = transform.position.x + gridOffest.x * cardScale;
		float y = transform.position.y + gridOffest.y * cardScale;
		return new Vector3 (x, y, -5);
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
