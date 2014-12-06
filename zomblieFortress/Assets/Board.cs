using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject wallFab;
	public GameObject towerFab;
	public static int widthx = 32;
	public static int widthy = 32;
	GameObject [,] board = new GameObject[widthx,widthy];
	List<Zombie> zombies = new List<Zombie>();
	List<Wall> walls = new List<Wall>();
	public static List<Point> zombiegridpos2D = new List<Point>();
	public static List<Point> wallgridpos2D = new List<Point>();

	private static int wall = 2;
	private static int wasteland = 1;
	private static int unsearched = 0;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < Board.widthx; x ++) {
			for (int y = 0; y < Board.widthy; y++) {
				GameObject wall = (GameObject) Instantiate(wallFab, new Vector3(x, y, 0), Quaternion.identity);
				board[x, y] = wall;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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

	}

	int[,] RecurseWasteland (int[,] landscape, Point start_point) {

		//determine whether the start point is wasteland
		if (this.board[start_point.x, start_point.y] is Wall){
			// If this cell is a wall, we stop searching.  Set this index to False (not wasteland)
			landscape[start_point.x, start_point.y] = Board.wall;
			return landscape;
		}

		// determine the next place to search.
		for (int cardinal_dir = 0; cardinal_dir < 4; cardinal_dir++){

			Point new_search_index = new Point();
			if (cardinal_dir == 0){ // new direction is N.
				new_search_index.x = start_point.x;
				new_search_index.y = start_point.y - 1;
			}
			else if (cardinal_dir == 1) { // new direction is W
				new_search_index.x = start_point.x - 1;
				new_search_index.y = start_point.y;
			}
			else if (cardinal_dir == 2) { // new direction is S
				new_search_index.x = start_point.x;
				new_search_index.y = start_point.y + 1;
			}
			else {  // new direction is E
				new_search_index.x = start_point.x + 1;
				new_search_index.y = start_point.y;
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
