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

	public List<GameObject> shadowSquares = new List<GameObject> ();
	public List<GameObject> inacticcveShadowSquares = new List<GameObject> ();


	private static int wall = 2;
	private static int wasteland = 1;
	private static int unsearched = 0;

	private long last_update;

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
		if (current_time >= this.last_update + 1000) {
			DetectFarmland();
			this.last_update = current_time;
		}

		if (hand.selected && Input.GetMouseButtonUp(0)) {
			Point gridPos =  worldPosToGridPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			SetShadowSquares(hand.selected.walls, hand.selected.towers, gridPos);
		}
	}

	void SetShadowSquares(List<Point> walls, List<Point> towers, Point gridPos){
		foreach(GameObject g in shadowSquares) {
			g.SetActive(false);
			shadowSquares.Remove(g);
			inacticcveShadowSquares.Add(g);
		}
		foreach(Point w in walls) {
			//GameObject g = 
		}
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
		Debug.Log("placing walls near: " + gridPos.x + ", " + gridPos.y + " like " + walls[0].x);
		List<Point> wallsToPlace = new List<Point> ();
		foreach (Point w in walls) {
			Point p = new Point(gridPos.x + w.x, gridPos.y + w.y);
			if (p.x >= 0 && p.x < widthx && p.y >= 0 && p.y < widthy && boardwall[p.x,p.y] == null){
				wallsToPlace.Add(p);
			}
			else{
				return false;
			}
		}
		foreach (Point p in wallsToPlace) {
			Vector3 pos = new Vector3(p.x - Board.widthx/2, p.y - Board.widthy/2, 0);
			GameObject wall = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
			boardwall[p.x, p.y] = wall;
			this.walls.Add(wall.GetComponent<Wall>());
		}
		
		return true;
	}

	bool[,] DetectFarmland () {
		Debug.Log ("Determining farms");
		int[,] wasteland = DetectWasteland();

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
	
	int[,] DetectWasteland () {
		// build up an empty 2d matrix for indicating which cells are wasteland.
		// initialize to false.

		// wasteland meanings:
		// 0 = not searched
		// 1 = has been searched, is wasteland
		// 2 = has been searched, is wall.
		int[,] wasteland = new int[Board.widthx, Board.widthy];

		// start out by starting from (0, 0) and investigating the board from there.
		//bool[,] wasteland = new bool[landscape.GetLength(0), landscape.GetLength (1)];
		Point start_point = new Point();
		if (this.zombies.Count == 0) {
			start_point.x = 0;
			start_point.y = 1;
			RecurseWasteland(ref wasteland, start_point);
		}
		else {
			foreach (Zombie live_zombie in this.zombies) {
				start_point.x = live_zombie.gridpos2D.x;
				start_point.y = live_zombie.gridpos2D.y;
				RecurseWasteland(ref wasteland, start_point);
			}
		}
		return wasteland;
	}

	// pretty-print the numeric value of an int matrix.
	void PrintMatrix(int[,] matrix){
		string row_string;
		for (int i = 0; i < matrix.GetLength(0); i++){
			row_string = "";
			for (int j = 0; j < matrix.GetLength(1); j++){
				row_string += " " + matrix[i, j];
			}
			Debug.Log (row_string);
		}
	}

	void RecurseWasteland (ref int[,] landscape, Point start_point) {
		//PrintMatrix (landscape);

		// If we've already positively confirmed this cell, skip.
		if (landscape[start_point.x, start_point.y] != Board.unsearched) {
			//Debug.Log ("Cell has previously been searched.");
			return;
		}
		else {
			// We're visiting this cell, so assume wasteland until we know this is a wall.
			if (this.boardwall[start_point.x, start_point.y] != null){
				//Debug.Log ("Cell is wall");
				landscape[start_point.x, start_point.y] = Board.wall;
				return;
			}
			else {
				// mark the board as having been visited.
				landscape[start_point.x, start_point.y] = Board.wasteland;
			}
		}

		
		// determine the next place to search.
		// Directions:
		//   1 | 0 | 7
		//   - + - + -
		//   2 | X | 6
		//   - + - + -
		//   3 | 4 | 5


		foreach (Point mask_values in this.adjacency_mask) {
			Point new_search_index = new Point();
			new_search_index.x = start_point.x + mask_values.x;
			new_search_index.y = start_point.y + mask_values.y;

			// Check boundary conditions.  Don't recurse there, if out of bounds.
			if (new_search_index.x < 0 || new_search_index.x >= landscape.GetLength (0) || new_search_index.y < 0 || new_search_index.y >= landscape.GetLength(1)){
				// do nothing ... we want to skip this.
				//Debug.Log("Skipping boundary condition");
			}
			else{
				RecurseWasteland(ref landscape, new_search_index);
			}
		} 
	}
}
