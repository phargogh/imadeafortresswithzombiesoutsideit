using UnityEngine;
using System.Collections;
using System;


public class GameLoop : MonoBehaviour {

	public Board board;
	public GameObject zombieFab;

	float tickLength = 0.1f;
	float timeSinceTick = 0f;
	int ticksElapsed = 0;
	int spawnNzombies = 1;
	int ticksTozombies = 50;
	int apocalypse = 1000000;

	// Use this for initialization
	void Start () {
		//MonoBehaviour.print("Game loop started");
		//ZombieApocalypse ();
		SpawnZombies(this.spawnNzombies);

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

		SpawnZombieTurn ();
		ZombieTurn ();
		TowerTurn ();

	}

	void SpawnZombieTurn(){
				this.ticksElapsed += 1;
				if (this.ticksElapsed == this.ticksTozombies) {
						int zN = UnityEngine.Random.Range (1, this.spawnNzombies + 1);
						//MonoBehaviour.print (zN.ToString () + " zombies spawning out of " + this.spawnNzombies.ToString ());
						SpawnZombies (zN);
						this.spawnNzombies += 1;
						this.ticksElapsed = 0;
				}
				this.apocalypse -= 1;
				if (UnityEngine.Random.Range (0, this.apocalypse)  == 0) {
						this.apocalypse = 1000000;
						ZombieApocalypse ();




		
				}
		}

	void SpawnZombies(int z){
		//MonoBehaviour.print("SpawnZombies running");

				int i = 0;
				int failcount = 0;
				int sresult;
				while (i < z) {

						sresult = SpawnZombie ();
						i += sresult;
						
						if (sresult == 0) {
							//MonoBehaviour.print("Error spawning zombie");
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

		return SpawnZomblieP(Spawngridpos2D);
		}
		
	int SpawnZomblieP(Point Spawngridpos2D){
		if (board.boardwall [Spawngridpos2D.x, Spawngridpos2D.y] == null & board.boardzombie [Spawngridpos2D.x, Spawngridpos2D.y] == null) {
			
			Vector3 pos = board.gridPointToWorldPos(Spawngridpos2D, 0);
			GameObject gzombie = (GameObject)Instantiate (zombieFab, pos, Quaternion.identity);
			Zombie czombie = gzombie.GetComponent<Zombie> ();
			czombie.init(board, Spawngridpos2D);
			board.zombies.Add(czombie);
			return 1;
			
		}
		return 0;
		
	}

	void ZombieApocalypse(){
		int r = 0;
		foreach (Point p in board.borderList) {
			r += SpawnZomblieP(p);
				}
		}
	}
