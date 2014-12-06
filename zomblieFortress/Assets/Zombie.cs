
using UnityEngine;
using System.Collections;
using System;

public class Zombie : MonoBehaviour {
	int x;
	int y;
	Point gridpos2D;
	Point targetgridpos2D;
	int xmax = Board.widthx - 1;
	int ymax = Board.widthy - 1;
	int searchx;
	int searchy;
	int directionx;
	int directiony;
	int attackrange;
	bool xmove = true;
	Board metaboardobj;

	
	public Zombie (Point gridpos2D, int attackrange, Board metaboardobj){
		this.gridpos2D = new Point(x, y);
		this.attackrange = attackrange;
		this.FindTargetDumbLoop();
		this.metaboardobj = metaboardobj;
		MonoBehaviour.print("A zombie is on the loose!");
		FindTargetDumbLoop ();

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
		
		foreach (Point p in metaboardobj.wallgridpos2D) {
			int cdistance = Math.Abs(this.gridpos2D.x - p.x) + Math.Abs(this.gridpos2D.y - p.y);
						if (cdistance <= distance) {
								distance = cdistance;
						}
				}
		candgridpos2D = this.gridpos2D;
		this.targetgridpos2D = candgridpos2D;
		this.targetgridpos2D.x = candgridpos2D.x;
		this.targetgridpos2D.y = candgridpos2D.y;
		DirectionUpdate();

	}
	bool Attackable (){
		if(metaboardobj.board[this.targetgridpos2D.x, this.targetgridpos2D.y].GetType() == typeof(Wall)){
			MonoBehaviour.print("Target acquired!");
		return true; // add query to board
		}
		else{
			return false;
		}
		}
	void FindTargetDumbLoop(){
		
		bool targetacquired = false;

		for (int i = 0; i <= this.xmax; i++){
			
			if (targetacquired){
				break;
			}
			else{
				for (int j = 0; j <= this.ymax; j++){
					if(Attackable()){ //need to actually define attackable or eliminate
						this.targetgridpos2D.x = i;
						this.targetgridpos2D.y = j;
						targetacquired = true;
						break;
					}
					
					
					
				}
			}
		}
	}
	
	
	
	void DirectionUpdate(){
		this.directionx = this.targetgridpos2D.x - this.gridpos2D.x;
		this.directiony = this.targetgridpos2D.y - this.gridpos2D.y;
	}

	void Attack(){
		// this.targetgridpos2D.x, this.targetgridpos2D.y, this.damage
		}

	void Move(){
		
		
		if (Math.Abs (this.targetgridpos2D.x) + Math.Abs (this.targetgridpos2D.y) <= this.attackrange) {
						this.Attack ();
						return;
				}

		if (this.gridpos2D.x == this.targetgridpos2D.x){
			this.xmove = false;
		}
		
		if (this.gridpos2D.y == this.targetgridpos2D.y){
			this.xmove = true; 
		}			
			
		if (this.xmove & this.gridpos2D.x != this.targetgridpos2D.x) {
				this.xmove = false;
			if(this.directionx > 0){
				this.gridpos2D.x += 1;
			}
			
			else{
				this.gridpos2D.x -= 1;
			}
				
				
			}
			
		else{
				this.xmove = true;
			if(this.directiony > 0){
				this.gridpos2D.x += 1;
			}
			
			else{
				this.gridpos2D.x -= 1;
			}
			
				this.DirectionUpdate();
			}
			
			
			
			
			
		}
		
	}
	

