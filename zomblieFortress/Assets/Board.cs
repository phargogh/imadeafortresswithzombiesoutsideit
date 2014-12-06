﻿using UnityEngine;
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


	// Use this for initialization
	void Start () {
		for (int x = 0; x < Board.widthx; x ++) {
			for (int y = 0; y < Board.widthy; y++) {
				Vector3 pos = new Vector3(x - Board.widthx/2, y - Board.widthy/2, 0);
				GameObject wall = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
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
		bool[,] wasteland = new bool[4, 4];
		for (int i = 0; i < Board.widthx; i++){
			for (int j = 0; j < Board.widthy; j++){
				Debug.Log(wasteland[i, j]);
			}
		}

		// start out by starting from (0, 0) and investigating the board from there.
		//bool[,] wasteland = new bool[landscape.GetLength(0), landscape.GetLength (1)];

	}

	bool[,] RecurseWasteland (bool[,] landscape, Point start_point) {
		//determine whether the start point is wasteland

		bool[,] wasteland = new bool[landscape.GetLength(0), landscape.GetLength (1)];
		for (int cardinal_dir = 0; cardinal_dir < 4; cardinal_dir++){

			Point new_search_index = new Point();
			// TODO: check the boundary cases.
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

			wasteland = this.RecurseWasteland(landscape, new_search_index);
		}
		return wasteland;
	}
}
