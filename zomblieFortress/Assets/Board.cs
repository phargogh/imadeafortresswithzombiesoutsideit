using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject wallFab;
	public GameObject towerFab;
	static int width = 32;
	GameObject [,] board = new GameObject[width,width];
	List<Zombie> zombies = new List<Zombie>();
	List<Wall> walls = new List<Wall>();
	List<Vector2> zombiexy = new List<Vector2>();
	List<Vector2> wallxy = new List<Vector2>();


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
