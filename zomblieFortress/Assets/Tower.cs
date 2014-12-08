using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	float damage = 25f;
	int cooldown = 12;
	int coolDownVariation = 3;
	int range = 5;

	Wall wall;

	int ticksToFire = 12;

	private LineRenderer line;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -120f));
	}

	public void init(Wall wall) {
		this.wall = wall;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeTurn(){
		line.enabled = false;
		ticksToFire--;
		if (ticksToFire < 0) {
			Zombie target = FindTarget();
			if (target) {
				ticksToFire = cooldown + Random.Range(0, coolDownVariation);
				//Debug.DrawLine(transform.position, target.transform.position, Color.yellow, 0.1f);
				target.TakeDamage(damage);
				line.enabled = true;
				//line.SetPosition(0, new Vector3(transform.position.x);
				line.SetPosition(1, target.transform.position);
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
