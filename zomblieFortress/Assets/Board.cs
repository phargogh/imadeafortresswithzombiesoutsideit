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
}
