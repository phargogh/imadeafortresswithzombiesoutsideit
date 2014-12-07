using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

	public Board board;
	public GameObject zombieFab;

	// Use this for initialization
	void Start () {



		Vector3 pos1 = new Vector3(2 , 9, 0);
		GameObject zombie = (GameObject) Instantiate(zombieFab, pos1, Quaternion.identity);
		Vector3 pos2 = new Vector3(4 , 9, 0);
		GameObject zombie2 = (GameObject) Instantiate(zombieFab, pos2, Quaternion.identity);

		Vector3 pos3 = new Vector3(9 , 9, 0);
		GameObject zombie3 = (GameObject) Instantiate(zombieFab, pos3, Quaternion.identity);
			
		Point zgridpos2D = new Point(9, 9);
		Zombie mydeadfriend = new Zombie (zgridpos2D, 1, board);


	
	}
	
	// Update is called once per frame
	void Update () {
		ZombieTurn ();
		TurretTurn ();
	 
	}

	void ZombieTurn(){
		foreach (Zombie z in board.zombies) {
			z.TakeTurn();

		}
	}

	void TurretTurn(){
		foreach (Tower t in board.towers) {
			t.TakeTurn();

		}
	}
}
