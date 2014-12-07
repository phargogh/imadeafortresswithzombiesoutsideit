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
	public Stack<GameObject> cardTowers = new Stack<GameObject> ();
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
		SetCost ();
		RefreshDisplay ();

	}

	void RefreshDisplay ()
	{
		foreach(GameObject g in cardSquares) {
			g.SetActive(false);
		}
		cardSquares.Clear ();
		
		foreach(GameObject g in cardTowers) {
			g.SetActive(false);
		}
		cardTowers.Clear ();

		foreach(Point w in walls) {
			Vector3 pos = GridOffsetToCardPos(w, -1);
			GameObject g = (GameObject) Instantiate(board.wallFab, new Vector3(), Quaternion.identity);
			g.transform.parent = this.transform;
			g.transform.localPosition = pos;
			g.transform.localScale = g.transform.localScale * cardScale;
			g.GetComponent<SpriteRenderer>().color = Color.white;
		}


		foreach(Point t in towers) {
			Vector3 pos = GridOffsetToCardPos(t, -2);
			GameObject g = (GameObject) Instantiate(board.towerFab, new Vector3(), Quaternion.identity);
			g.transform.parent = this.transform;
			g.transform.localPosition = pos;
			g.transform.localScale = g.transform.localScale * cardScale;
			g.GetComponent<SpriteRenderer>().color = Color.yellow;
		}
	}

	Vector3 GridOffsetToCardPos(Point gridOffest, float z) {
		float x = gridOffest.x * cardScale / transform.localScale.x;
		float y = gridOffest.y * cardScale / transform.localScale.y;
		return new Vector3 (x, y, z);
	}

	void SetWalls() {
		int num_walls = rand.Next (3, 8);
		walls = CardGen.createShape (num_walls);
	}

	void SetTowers (){
		int num_towers = rand.Next (0,2);
		for (int i = 0; i < num_towers; i++){
			this.towers.Add(CardGen.ChoosePoint(this.walls));
		};
	}

	void SetCost (){
		int base_cost = 5;
		int wall_cost = 1;
		int tower_cost = 5;
		this.cost = base_cost + wall_cost*this.walls.Count + tower_cost*this.towers.Count;
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
		//Debug.Log ("This card was clicked");
		if (isSelected) {
			hand.Unselect();
		} else {
			hand.SelectCard(this);
		}
	}
}
