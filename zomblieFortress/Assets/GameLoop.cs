using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

	public Board board;

	// Use this for initialization
	void Start () {
		Board gameboard = new Board();
		Point zgridpos2D = new Point(3, 5);
		Zombie mydeadfriend = new Zombie (zgridpos2D, 1, gameboard);


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
