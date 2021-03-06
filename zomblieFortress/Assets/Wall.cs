﻿using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public float health = 100f; //NOT REAL
	Tower tower = null;
	public Point gridpos2D;
	public Board board;

	// Use this for initialization
	void Start () {
		this.health = 1000f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage(float damage) {
		this.health -= damage;
		if (this.health < 0f) {
			//MonoBehaviour.print("This wall is dead");
			if (Board.gameBoard.boardwall[this.gridpos2D.x,this.gridpos2D.y] != this) { // test only code
				Debug.LogError("***** A Wall is lost *****");
			}
			Board.gameBoard.boardwall[this.gridpos2D.x,this.gridpos2D.y]= null;
			Board.gameBoard.walls.Remove(this);
			if (tower) {
				Destroy(tower.gameObject);
			}
			Destroy(gameObject);
		}

	}

	public void SetTower(Tower tower) {
		this.tower = tower;
		tower.init(this);
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
