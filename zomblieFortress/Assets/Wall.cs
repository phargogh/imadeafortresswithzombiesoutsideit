using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public float health = 100f;
	Tower tower = null;
	public Point gridpos2D;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage(float damage) {
		this.health -= damage;
		if (this.health < 0f) {
			MonoBehaviour.print("This wall is dead");
				}

	}

	void RemoveDeadWall(){
		//board.gridpos2D;
		}




}
