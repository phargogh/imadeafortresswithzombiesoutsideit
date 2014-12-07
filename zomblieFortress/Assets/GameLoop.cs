using UnityEngine;
using System.Collections;
using System;


public class GameLoop : MonoBehaviour {

	public Board board;
	public GameObject zombieFab;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		SpawnZombieTurn ();
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

	void Tick(){
				
				DateTime stime =  DateTime.Now;
				ZombieTurn ();
				TurretTurn ();
		}

	void SpawnZombieTurn(){

		/*
		comp.gameObject.transform.position = new Vector3 ();
		Vector3 pos2 = new Vector3(4 , 9, 0);
		GameObject zombie2 = (GameObject) Instantiate(zombieFab, pos2, Quaternion.identity);
		
		Vector3 pos3 = new Vector3(9 , 9, 0);
		GameObject zombie3 = (GameObject) Instantiate(zombieFab, pos3, Quaternion.identity);
		
		Point zgridpos2D = new Point(9, 9);
		Zombie mydeadfriend = new Zombie (zgridpos2D, 1, board);
		*/
		}

	void SpawnZombies(int z){
		int i = 0;
		while(i < z){
			i += SpawnZombie()



	int SpawnZombie(){
				int x;
				int y;
				Point gridpos2D;


				if (UnityEngine.Random.Range (0, 1) == 1) {
						gridpos2D.x = UnityEngine.Random.Range (0, Board.widthx - 1);
						if (UnityEngine.Random.Range (0, 1) == 1) {
								gridpos2D.y = Board.widthy - 1;
						} else {
								gridpos2D.y = 0;
						}
				} else {
						gridpos2D.y = UnityEngine.Random.Range (0, Board.widthy - 1);

						if (UnityEngine.Random.Range (0, 1) == 1) {
								gridpos2D.x = Board.widthy - 1;
						} else {
								gridpos2D.x = 0;
						}
				}

		if (board.boardwall [gridpos2D.x, gridpos2D.y] == null & board.boardzombie [gridpos2D.x, gridpos2D.y]) {

						Vector3 pos = new Vector3 (gridpos2D.x, gridpos2D.y, 0);
						GameObject gzombie = (GameObject)Instantiate (zombieFab, pos, Quaternion.identity);
						Zombie czombie = gzombie.GetComponent<Zombie> ();
						czombie.gridpos2D = gridpos2D;
						board.zombies.Add(czombie);
						return 1;

				}
		
		}
		return 0;



	}
