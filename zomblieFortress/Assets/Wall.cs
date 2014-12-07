using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public float health = 100f;
	Tower tower = null;
	public Point gridpos2D;
	public Board board;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage(float damage) {
		this.health -= damage;
		if (this.health < 0f) {
			//MonoBehaviour.print("This wall is dead");
			Board.gameBoard.boardwall[this.gridpos2D.x,this.gridpos2D.y]= null;
			Board.gameBoard.walls.Remove(this);
			Destroy(gameObject);
		}

	}

	public void SetTower(Tower tower) {
		tower.init(this);
		this.tower = tower;
	}

	public void TakeTurn() {
		if (tower) {
			tower.TakeTurn();
		}
	}

	void RemoveDeadWall(){
		//board.gridpos2D;
		}




}
