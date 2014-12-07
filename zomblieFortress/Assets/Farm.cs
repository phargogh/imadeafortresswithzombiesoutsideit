using UnityEngine;
using System.Collections;

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
