using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject wallFab;
	public GameObject towerFab;
	public static int widthx = 32;
	public static int widthy = 32;
	public GameObject [,] board = new GameObject[widthx,widthy];
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
		List<Point> start_walls = new List<Point>();
		foreach (Point p in adjacency_mask) {
			start_walls.Add(center+p);
		}

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

	void spawnWalls(List<Point> walls, List<Point> towers){
		foreach (Point w in walls) {
			Vector3 pos = new Vector3(w.x - Board.widthx/2, w.y - Board.widthy/2, 0);
			GameObject wall = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
			if (board[w.x,w.y] == null){
				board[w.x, w.y] = wall;	
			}
			else{
				Debug.Log("Tried to place a wall on an occupied space:",wall);
			}
		}
	}
	
	void DetectWasteland () {
		// build up an empty 2d matrix for indicating which cells are wasteland.
		// initialize to false.
		// TODO: use Board.widthx, Board.widthy

		// wasteland meanings:
		// 0 = not searched
		// 1 = has been searched, is wasteland
		// 2 = has been searched, is wall.
		int[,] wasteland = new int[4, 4];
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
		PrintMatrix(RecurseWasteland(wasteland, start_point));
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

	int[,] RecurseWasteland (int[,] landscape, Point start_point) {

		//determine whether the start point is wasteland
		if (this.board[start_point.x, start_point.y] is Wall){
			// If this cell is a wall, we stop searching.  Set this index to False (not wasteland)
			landscape[start_point.x, start_point.y] = Board.wall;
			return landscape;
		}

		// determine the next place to search.
		// Directions:
		//   1 | 0 | 7
		//   - + - + -
		//   2 | X | 6
		//   - + - + -
		//   3 | 4 | 5


		for (int direction = 0; direction < 8; direction++){

			Point new_search_index = new Point();
			if (direction == 0){ // new direction is N.
				new_search_index.x = start_point.x;
				new_search_index.y = start_point.y - 1;
			}
			else if (direction == 1) { // NW
				new_search_index.x = start_point.x - 1;
				new_search_index.y = start_point.y - 1;
			}
			else if (direction == 2) { // new direction is W
				new_search_index.x = start_point.x - 1;
				new_search_index.y = start_point.y;
			}
			else if (direction == 3) { // SW
				new_search_index.x = start_point.x - 1;
				new_search_index.y = start_point.y + 1;
			}
			else if (direction == 4) { // new direction is S
				new_search_index.x = start_point.x;
				new_search_index.y = start_point.y + 1;
			}
			else if (direction == 5) { // SE
				new_search_index.x = start_point.x + 1;
				new_search_index.y = start_point.y + 1;
			}
			else if (direction == 6) {  // new direction is E
				new_search_index.x = start_point.x + 1;
				new_search_index.y = start_point.y;
			}
			else {  // NE
				new_search_index.x = start_point.x + 1;
				new_search_index.y = start_point.y - 1;
			}

			// Check boundary conditions.  Don't recurse there, if out of bounds.
			if (new_search_index.x < 0 || new_search_index.x > landscape.GetLength (0) || new_search_index.y < 0 || new_search_index.y > landscape.GetLength(1)){
				return landscape;
			}

			landscape = this.RecurseWasteland(landscape, new_search_index);
		}
		return landscape;
	}
}
