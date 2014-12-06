using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject wallFab;
	public GameObject towerFab;
	public static int widthx = 32;
	public static int widthy = 32;
	public GameObject [,] boardwall = new GameObject[widthx,widthy];
	public GameObject [,] boardzombie = new GameObject[widthx,widthy];
	List<Zombie> zombies = new List<Zombie>();
	List<Wall> walls = new List<Wall>();
	public List<Point> zombiegridpos2D = new List<Point>();
	public List<Point> wallgridpos2D = new List<Point>();

	private static int wall = 2;
	private static int wasteland = 1;
	private static int unsearched = 0;

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

		
		spawnWalls (start_walls, start_walls);
		spawnWalls (corners,corners);
		DetectWasteland();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool spawnWalls(List<Point> walls, List<Point> towers){
		foreach (Point w in walls) {
			if (boardwall[w.x,w.y] == null){
				Vector3 pos = new Vector3(w.x - Board.widthx/2, w.y - Board.widthy/2, 0);
				GameObject wall = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
				boardwall[w.x, w.y] = wall;
				this.walls.Add(wall.GetComponent<Wall>());
			}
			else{
				Debug.Log("Tried to place a wall on an occupied space: " + w.x + ", " + w.y );
				return false;
			}
		}
		return true;
	}
	
	void DetectWasteland () {
		// build up an empty 2d matrix for indicating which cells are wasteland.
		// initialize to false.
		// TODO: use Board.widthx, Board.widthy

		// wasteland meanings:
		// 0 = not searched
		// 1 = has been searched, is wasteland
		// 2 = has been searched, is wall.
		int[,] wasteland = new int[Board.widthx, Board.widthy];
		//int[,] wasteland = new int[8, 8];
		for (int i = 0; i < wasteland.GetLength (0); i++){
			for (int j = 0; j < wasteland.GetLength (1); j++){
				Debug.Log(wasteland[i, j]);
			}
		}

		// start out by starting from (0, 0) and investigating the board from there.
		//bool[,] wasteland = new bool[landscape.GetLength(0), landscape.GetLength (1)];
		Point start_point = new Point();
		start_point.x = 0;
		start_point.y = 1;
		RecurseWasteland(ref wasteland, start_point);
		PrintMatrix(wasteland);
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
