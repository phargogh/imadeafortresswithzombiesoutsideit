using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

	public Board board;

	// Use this for initialization
	void Start () {

		Point zgridpos2D = new Point(9, 9);
		Zombie mydeadfriend = new Zombie (zgridpos2D, 1, board);


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
