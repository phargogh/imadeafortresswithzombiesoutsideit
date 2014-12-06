using UnityEngine;
using System.Collections;

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
	
	Zombie (int x, int y, int attackrange){
		this.x = x;
		this.y = y;
		this.gridpos2D = new Point(x, y);
		this.attackrange = attackrange;
		this.FindTargetDumbLoop();
	}
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FindTargetClosest(){
	Point candgridpos2D;
	int distance = this.xmax + this.ymax;
		
		foreach (Point p in Board.wallgridpos2D) {
						int cdistance = Abs (p - this.gridpos2D).Sum (); 
						if (cdistance <= distance) {
								distance = cdistance;
								candgridpos2D = p;
						}
				}
		
		this.targetgridpos2D = candgridpos2D;
		this.targetx = candgridpos2D[0];
		this.targety = candgridpos2D[1];

	}
	
	void FindTargetDumbLoop(){
		
		bool targetacquired = false;
		
		for (int i = 1; i <= this.xmax; i++){
			
			if (targetacquired){
				break;
			}
			else{
				for (int j = 1; j <= this.ymax; j++){
					if(attackable(i,j)){
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
		
		if (Abs(this.targetx) + Abs(this.targety) <= this.attackrange){
			this.Attack();
				return;
			
			
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
	
}

