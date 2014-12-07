using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject wallFab;
	public GameObject towerFab;
	public GameObject farmFab;
	public Hand hand;
	public static int widthx = 32;
	public static int widthy = 32;
	public GameObject [,] boardwall = new GameObject[widthx,widthy];
	public GameObject [,] boardzombie = new GameObject[widthx,widthy];
	public List<GameObject> farms = new List<GameObject>();
	public List<Zombie> zombies = new List<Zombie>();
	public List<Wall> walls = new List<Wall>();
	public List<Tower> towers = new List<Tower>();
	public List<Point> zombiegridpos2D = new List<Point>();
	public List<Point> wallgridpos2D = new List<Point>();

	public Stack<GameObject> shadowSquares = new Stack<GameObject> ();
	public Stack<GameObject> inacticcveShadowSquares = new Stack<GameObject> ();

	private long last_update;
	private bool trigger_farm_detection ;

	private List<Point> adjacency_mask = new List<Point>(){
		new Point(0,-1),
		new Point(-1,0),
		new Point(0,1),
		new Point(1,0),
		new Point(-1,-1),
		new Point(-1,1),
		new Point(1,-1),
		new Point(1,1),
	};
	
	// Use this for initialization
	void Start () {
		Point center = new Point (16, 16);
		List<Point> start_walls = CardGen.getAdjacent (center);


		List<Point> corners = new List<Point> (){
			new Point (0, 0),
			new Point (0, 31),
			new Point (31, 0),
			new Point (31, 31),
		};
		
		spawnWalls (start_walls, start_walls, new Point());
		spawnWalls (corners,corners, new Point());
		DetectFarmland();

	}
	
	// Update is called once per frame
	void Update () {
		long num_ticks = DateTime.Now.Ticks;
		long current_time = num_ticks / 10000;  // time in ms
		if (current_time >= this.last_update + 1000 || this.trigger_farm_detection == true) {
			DetectFarmland();
			this.last_update = current_time;
			this.trigger_farm_detection = false;  // reset so we don't re-detect farmland
		}

		if (hand.selected) {
			Point gridPos =  worldPosToGridPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			SetShadowSquares(hand.selected.walls, hand.selected.towers, gridPos);
		} else {
			SetShadowSquares(new List<Point>(), new List<Point>(), new Point());
		}
	}

	void SetShadowSquares(List<Point> walls, List<Point> towers, Point gridPos){
		//Debug.Log(" ----------------------------------------------------- " + gridPos.x + " " + gridPos.y);

		foreach(GameObject g in shadowSquares) {
			g.SetActive(false);
			//shadowSquares.Remove(g);
			//g.GetComponent<SpriteRenderer>().color = Color.clear;
			//g.transform.position = new Vector3(-100, -100, 100); 
			//Destroy(g);
			inacticcveShadowSquares.Push(g);
		}
		shadowSquares.Clear();
		foreach(Point w in walls) {
			int x = gridPos.x + w.x;
			int y = gridPos.y + w.y;
			GameObject g = inacticcveShadowSquares.Count > 0 ? inacticcveShadowSquares.Pop() : (GameObject) Instantiate(wallFab, new Vector3(), Quaternion.identity);
			//GameObject g = (GameObject) Instantiate(wallFab, new Vector3(), Quaternion.identity);
			g.SetActive(true);
			g.transform.position = gridPointToWorldPos(new Point(x, y), -1);
			Color c = (x >= 1 && x < widthx-1 && y >= 1 && y < widthy-1 && boardwall[x,y] == null) ? (boardzombie[x,y] == null ? Color.white : Color.magenta) : Color.red;
			g.GetComponent<SpriteRenderer>().color = c;
			shadowSquares.Push(g);
		}
		// TODO: check for zombies or towers?
	}

	public Point worldPosToGridPoint(Vector3 worldPos) {
		Point gridPos = new Point ();
		gridPos.x = Mathf.RoundToInt(worldPos.x - transform.position.x) + widthx/2;
		gridPos.y = Mathf.RoundToInt(worldPos.y - transform.position.y) + widthy/2;
		return gridPos;
	}

	public Vector3 gridPointToWorldPos(Point gridPoint, float z) {
		Vector3 worldPos = new Vector3 ();
		worldPos.x = transform.position.x + gridPoint.x - widthx / 2;
		worldPos.y = transform.position.y + gridPoint.y - widthy / 2;
		worldPos.z = z;
		return worldPos;
	}

	public bool spawnWalls(List<Point> walls, List<Point> towers, Point gridPos){
		//Debug.Log("placing walls near: " + gridPos.x + ", " + gridPos.y + " like " + walls[0].x);
		List<Point> wallsToPlace = new List<Point> ();
		foreach (Point w in walls) {
			Point p = new Point(gridPos.x + w.x, gridPos.y + w.y);
			if (p.x >= 1 && p.x < widthx-1 && p.y >= 1 && p.y < widthy-1 && boardwall[p.x,p.y] == null){
				wallsToPlace.Add(p);
			}
			else{
				return false;
			}
		}
		foreach (Point p in wallsToPlace) {
			placeWall(p);
		}
		this.trigger_farm_detection = true;  // trigger farmland to be re-detected.
		return true;
	}

	void placeWall(Point p){
		Vector3 pos = new Vector3(p.x - Board.widthx/2, p.y - Board.widthy/2, 0);
		GameObject wall = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
		boardwall[p.x, p.y] = wall;
		this.walls.Add(wall.GetComponent<Wall>());
		wall.GetComponent<Wall> ().gridpos2D = p;
	}

	bool[,] DetectFarmland () {
		Debug.Log ("Determining farms");
		int[,] wasteland = Farm.DetectWasteland(this);

		// remove all the farm sites that were set up in the last frame.
		foreach (GameObject farm_site in this.farms) {
			Destroy(farm_site);
		}
		this.farms = new List<GameObject>();

		// farmland is anything that's not wasteland and not walls.
		bool[,] farmland = new bool[wasteland.GetLength(0), wasteland.GetLength(1)];
		for (int row = 0; row < wasteland.GetLength(0); row++) {
			for (int col = 0; col < wasteland.GetLength(1); col++) {
				if (wasteland[row, col] == 0 && this.boardwall[row, col] == null) {
					farmland[row, col] = true;

					Vector3 pos = new Vector3(row - Board.widthx/2, col - Board.widthy/2, 0);
					GameObject farm = (GameObject) Instantiate(farmFab, pos, Quaternion.identity);
					this.farms.Add (farm);

				}
			}
		}
		return farmland;
	}
}
