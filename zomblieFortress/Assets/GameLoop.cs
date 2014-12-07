using UnityEngine;
using System.Collections;
using System;


public class GameLoop : MonoBehaviour {

	public Board board;
	public GameObject zombieFab;

	float tickLength = 0.1f;
	float timeSinceTick = 0f;

	// Use this for initialization
	void Start () {
		MonoBehaviour.print("Game loop started");
		SpawnZombieTurn ();

	}
	
	// Update is called once per frame
	void Update () {
		//MonoBehaviour.print("Game loop started");
		//SpawnZombieTurn ();
		//ZombieTurn ();

		timeSinceTick += Time.deltaTime;
		if (timeSinceTick > tickLength) {
			timeSinceTick -= tickLength;
			Tick();
		}
	}

	void ZombieTurn(){
		//MonoBehaviour.print("Zombie turn");
		foreach (Zombie z in board.zombies) {
			z.TakeTurn();
		}
	}

	void TowerTurn(){
		foreach (Wall w in board.walls) {
			w.TakeTurn();
		}
	}

	void Tick(){
				
		//SpawnZombieTurn ();
		ZombieTurn ();
		TowerTurn ();

	}

	void SpawnZombieTurn(){
		print("SpawnZombieTurn");

		SpawnZombies(10);

		/*
		comp.gameObject.transform.position = new Vector3 ();
		Vector3 pos2 = new Vector3(4 , 9, 0);
		GameObject zombie2 = (GameObject) Instantiate(zombieFab, pos2, Quaternion.identity);
		
		Vector3 pos3 = new Vector3(9 , 9, 0);
		GameObject zombie3 = (GameObject) Instantiate(zombieFab, pos3, Quaternion.identity);
		
		Point zSpawngridpos2D = new Point(9, 9);
		Zombie mydeadfriend = new Zombie (zSpawngridpos2D, 1, board);
		*/
		}

	void SpawnZombies(int z){
		MonoBehaviour.print("SpawnZombies running");

				int i = 0;
				int failcount = 0;
				int sresult;
				while (i < z) {

						sresult = SpawnZombie ();
						i += sresult;
						
						if (sresult == 0) {
							MonoBehaviour.print("Error spawning zombie");
								failcount += 1;
								if (failcount > 30) {
										return;
								}
						}
				}
		}



	int SpawnZombie(){
				Point Spawngridpos2D;


				if (UnityEngine.Random.Range (0, 2) == 1) {
						Spawngridpos2D.x = UnityEngine.Random.Range (0, Board.widthx - 1);
						if (UnityEngine.Random.Range (0, 2) == 1) {
								Spawngridpos2D.y = Board.widthy - 1;
						} else {
								Spawngridpos2D.y = 0;
						}
				} else {
						Spawngridpos2D.y = UnityEngine.Random.Range (0, Board.widthy - 1);

						if (UnityEngine.Random.Range (0, 2) == 1) {
								Spawngridpos2D.x = Board.widthy - 1;
						} else {
								Spawngridpos2D.x = 0;
						}
				}

		//MonoBehaviour.print (Spawngridpos2D.x.ToString() + ',' + Spawngridpos2D.y.ToString());
		//MonoBehaviour.print (board.boardwall [Spawngridpos2D.x, Spawngridpos2D.y]);

		if (board.boardwall [Spawngridpos2D.x, Spawngridpos2D.y] == null & board.boardzombie [Spawngridpos2D.x, Spawngridpos2D.y] == null) {

						Vector3 pos = board.gridPointToWorldPos(Spawngridpos2D, 0);
						GameObject gzombie = (GameObject)Instantiate (zombieFab, pos, Quaternion.identity);
						Zombie czombie = gzombie.GetComponent<Zombie> ();
						czombie.attackrange = 1;
						czombie.attackdamage = 10f;
						czombie.metaboard = board;
						czombie.gridpos2D = Spawngridpos2D;
						czombie.oldgridpos2D = Spawngridpos2D;
						czombie.needtarget = true;
						czombie.UpdateZombieBoard();
						board.zombies.Add(czombie);
						return 1;

				}
		return 0;
		}
		



	}
