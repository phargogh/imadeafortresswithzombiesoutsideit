using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	float damage = 25f;
	int cooldown = 12;
	int coolDownVariation = 3;
	int range = 5;

	Wall wall;

	int ticksToFire = 12;

	// Use this for initialization
	void Start () {
	
	}

	public void init(Wall wall) {
		this.wall = wall;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeTurn(){
		ticksToFire--;
		if (ticksToFire < 0) {
			Zombie target = FindTarget();
			if (target) {
				Debug.Log("************* Found Target **************");
				ticksToFire = cooldown + Random.Range(0, coolDownVariation);
				Debug.DrawLine(transform.position, target.transform.position, Color.yellow, 0.1f);

			}
		}
	}

	Zombie FindTarget() {
		int minDist = 99;
		Zombie closest = null;
		foreach(Zombie z in Board.gameBoard.zombies) {
			int dist = Point.ManhattanDistance(z.gridpos2D, wall.gridpos2D);
			if (!closest || dist < minDist) {
				closest = z;
				minDist = dist;
			}

		}
		return minDist <= range ? closest : null;
	}
}
