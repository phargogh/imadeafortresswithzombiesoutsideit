using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Board gameboard = new Board();
		Zombie mydeadfriend = new Zombie (42, 4, 1, gameboard);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
