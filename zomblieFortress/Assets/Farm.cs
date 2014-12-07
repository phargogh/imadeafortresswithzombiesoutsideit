using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Farm : MonoBehaviour {

	public static int wall = 2;
	private static int wasteland = 1;
	private static int unsearched = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

    public static List<Point> BoundaryPixels () {
        List<int> xrange = new List<int>();
        for (int i = 0; i < Board.widthx; i++) {
            xrange.Add(i);
        }

        List<int> yrange = new List<int>();
        for (int i = 0; i < Board.widthx; i++) {
            yrange.Add(i);
        }

        List<Point> boundary_points = new List<Point>();
        // North, south walls
        foreach (int i in xrange) {
            Point n = new Point(i, 0);
            Point s = new Point(i, Board.widthy - 1);

            boundary_points.Add(n);
            boundary_points.Add(s);
        }
        
        // West, East walls
        foreach (int i in yrange) {
            Point w = new Point(0, i);
            Point e = new Point(Board.widthx - 1, i);

            boundary_points.Add(w);
            boundary_points.Add(e);
        }
        return boundary_points;
    }

	public static bool[,] DetectFarmland (Board board) {
		Debug.Log ("Determining farms");
		int[,] wasteland = Farm.DetectWasteland(board);

		// remove all the farm sites that were set up in the last frame.
		foreach (GameObject farm_site in board.farms) {
			Destroy(farm_site);
		}
		board.farms = new List<GameObject>();

		// farmland is anything that's not wasteland and not walls.
		bool[,] farmland = new bool[wasteland.GetLength(0), wasteland.GetLength(1)];
		for (int row = 0; row < wasteland.GetLength(0); row++) {
			for (int col = 0; col < wasteland.GetLength(1); col++) {
				if (wasteland[row, col] == 0 && board.boardwall[row, col] == null) {
					farmland[row, col] = true;

					Vector3 pos = new Vector3(row - Board.widthx/2, col - Board.widthy/2, 0);
					GameObject farm = (GameObject) Instantiate(board.farmFab, pos, Quaternion.identity);
					board.farms.Add (farm);

				}
			}
		}
		return farmland;
	}

	public static int[,] DetectWasteland (Board board) {
		// build up an empty 2d matrix for indicating which cells are wasteland.
		// initialize to false.

		// wasteland meanings:
		// 0 = not searched
		// 1 = has been searched, is wasteland
		// 2 = has been searched, is wall.
		int[,] wasteland = new int[Board.widthx, Board.widthy];


        // Create a list of points to use as starting points for our farms search.
		List<Point> starting_points = new List<Point>();
		foreach (Zombie zom in board.zombies) {
            Point z_point = new Point(zom.gridpos2D.x, zom.gridpos2D.y);
			starting_points.Add(z_point);
		}

        foreach (Point boundary in BoundaryPixels()){
            starting_points.Add(boundary);
        }

        // Search for farms.
        foreach (Point starting_point in starting_points) {
            Farm.RecurseWasteland(ref wasteland, starting_point, board.boardwall);
        }
		return wasteland;
	}

	public static void RecurseWasteland (ref int[,] landscape, Point start_point, GameObject[,] boardwall) {
		//PrintMatrix (landscape);

		// If we've already positively confirmed this cell, skip.
		if (landscape[start_point.x, start_point.y] != Farm.unsearched) {
			//Debug.Log ("Cell has previously been searched.");
			return;
		}
		else {
			// We're visiting this cell, so assume wasteland until we know this is a wall.
			if (boardwall[start_point.x, start_point.y] != null){
				//Debug.Log ("Cell is wall");
				landscape[start_point.x, start_point.y] = Farm.wall;
				return;
			}
			else {
				// mark the board as having been visited.
				landscape[start_point.x, start_point.y] = Farm.wasteland;
			}
		}

		
		// determine the next place to search.
		// Directions:
		//   1 | 0 | 7
		//   - + - + -
		//   2 | X | 6
		//   - + - + -
		//   3 | 4 | 5


		foreach (Point new_search_index in CardGen.getAdjacent(start_point)) {
			// Check boundary conditions.  Don't recurse there, if out of bounds.
			if (new_search_index.x < 0 || new_search_index.x >= landscape.GetLength (0) || new_search_index.y < 0 || new_search_index.y >= landscape.GetLength(1)){
				// do nothing ... we want to skip this.
				//Debug.Log("Skipping boundary condition");
			}
			else{
				RecurseWasteland(ref landscape, new_search_index, boardwall);
			}
		} 
	}
}
