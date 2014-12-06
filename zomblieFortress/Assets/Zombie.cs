
using UnityEngine;
using System.Collections;
using System;

public class Zombie : MonoBehaviour {
	int x;
	int y;
	Point gridpos2D;
	int targetx;
	int targety;
	Point targetgridpos2D;
	int xmax = Board.widthx;
	int ymax = Board.widthy;
	int searchx;
	int searchy;
	int directionx;
	int directiony;
	int attackrange;
	bool xmove = true;
	Board gameboard;

	
	Zombie (int x, int y, int attackrange, Board gameboard){
		this.x = x;
		this.y = y;
		this.gridpos2D = new Point(x, y);
		this.attackrange = attackrange;
		this.FindTargetDumbLoop();
		this.gameboard = gameboard;
	}
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FindTargetClosest(){
		Point candgridpos2D; // = Board.wallgridpos2D[0];
		int distance = xmax + ymax; 
		
		foreach (Point p in gameboard.wallgridpos2D) {
			int cdistance = Math.Abs(this.gridpos2D.x - p.x) + Math.Abs(this.gridpos2D.y - p.y);
						if (cdistance <= distance) {
								distance = cdistance;
						}
				}
		candgridpos2D = this.gridpos2D;
		this.targetgridpos2D = candgridpos2D;
		this.targetx = candgridpos2D.x;
		this.targety = candgridpos2D.y;

	}
	bool attackable (int x, int y){
		return true; // add query to zombie
		}
	void FindTargetDumbLoop(){
		
		bool targetacquired = false;

		for (int i = 1; i <= this.xmax; i++){
			
			if (targetacquired){
				break;
			}
			else{
				for (int j = 1; j <= this.ymax; j++){
					if(attackable(i,j)){ //need to actually define attackable or eliminate
						this.targetx = i;
						this.targety = j;
						targetacquired = true;
						break;
					}
					
					
					
				}
			}
		}
	}
	
	
	
	void DirectionUpdate(){
		this.directionx = this.targetx - this.x;
		this.directiony = this.targety - this.y;
	}

	void Attack(){
		// this.targetx, this.targety, this.damage
		}

	void Move(){
		
		if (this.x == this.targetx){
			this.xmove = false;
		}
		
		if (this.y == this.targety){
			this.xmove = true; 
		}
		
		if (Math.Abs (this.targetx) + Math.Abs (this.targety) <= this.attackrange) {
						this.Attack ();
						return;
				}
			
			
		if (this.xmove & this.x != this.targetx) {
				this.xmove = false;
			if(this.directionx > 0){
				this.x += 1;
			}
			
			else{
				this.x -= 1;
			}
				
				
			}
			
		else{
				this.xmove = true;
			if(this.directiony > 0){
				this.x += 1;
			}
			
			else{
				this.x -= 1;
			}
			
				this.DirectionUpdate();
			}
			
			
			
			
			
		}
		
	}
	

