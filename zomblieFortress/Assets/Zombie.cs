using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	int x;
	int y;
	Vector2 xy
	int targetx;
	int targety;
	Vector2 xy = Vector2(targetx,targety)
	int xmax = getboardx ();
	int ymax = getboardy ();
	int searchx;
	int searchy;
	int attackrange;
	
	Zombie (int x, int y, int attackrange){
		this.x = x;
		this.y = y;
		this.xy = Vector2(x,y)
			this.attackrange = attackrange
				this.FindTarget();
	}
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FindTargetClosest();
	Vector2 cvector;
	int distance = this.xmax + this.ymax
		
		foreach (Vector2 p in Board.wallxy) 
	{
		int cdistance = Abs(p - this.xy).Sum()
		if(cdistance <= distance){
			cvector = p
		}
		
		this.targetxy = cvector
		this.targetx = cvector[0]
		this.targety = cvector[1]
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
						this.targetx = i
							this.targety = j
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
	void Move(){
		
		if (this.x == this.targetx){
			this.xmove = false 
		}
		
		if (this.y == this.targety){
			this.xmove = true 
		}
		
		if (Abs(this.targetx) + Abs(this.targety) <= this.attackrange){
			this.attack(targetx, targety)
				break; //this needs to be the equivalent of a return statement that breaks the function here.
			
			
			if (this.xmove & this.x != this.targetx) {
				this.xmove = false
				if(this.directionx > 0){
					this.x += 1;
				}
				
				else{
					this.x -= 1;
				}
				
				
			}
			
			else{
				this.xmove = true
				if(this.directiony > 0){
					this.x += 1;
				}
				
				else{
					this.x -= 1;
				}
				
				this.DirectionUpdate()
			}
			
			
			
			
			
		}
		
	}
	
}

