
using UnityEngine;
using System.Collections;
using System;

public class Zombie : MonoBehaviour {
	int x;
	int y;
	public Point gridpos2D;
	Point targetgridpos2D;
	int xmax = Board.widthx;
	int ymax = Board.widthy;
	int searchx;
	int searchy;
	int targetdistancex;
	int targetdistancey;
	int attackrange;
	bool xmove = true;
	Board metaboardobj;

	
	public Zombie (Point gridpos2D, int attackrange, Board metaboardobj){
		this.gridpos2D = gridpos2D;
		this.metaboardobj = metaboardobj;
		this.attackrange = attackrange;
		MonoBehaviour.print("A zombie is on the loose!");
		this.FindTargetDumbLoop ();
		this.PrintZombiePosition ();
		MonoBehaviour.print (this.targetgridpos2D.x);
	
		//MonoBehaviour.print (Math.Abs (this.targetdistancex) + Math.Abs (this.targetdistancey) > this.attackrange);
		//MonoBehaviour.print (Math.Abs (this.targetdistancex));
		//MonoBehaviour.print (Math.Abs (this.targetdistancey));
		//MonoBehaviour.print (this.attackrange);
		int counter = 1;
		while (Math.Abs (this.targetdistancex) + Math.Abs (this.targetdistancey) > this.attackrange & counter < 50) {

			this.Move();
			counter += 1;
			//this.PrintZombiePosition ();

				}

		
	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//this.Move ();
		
	}
	
	void FindTargetClosest(){
		Point candgridpos2D;
		int distance = xmax + ymax; 
		

		foreach (Point p in this.metaboardobj.wallgridpos2D) {

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

				if (this.metaboardobj.boardwall [this.targetgridpos2D.x, this.targetgridpos2D.y] == null) {
						return false;
				}

		return true;
		}

	void FindTargetDumbLoop(){

		for (int i = 0; i <= this.xmax; i++){


				for (int j = 0; j <= this.ymax; j++){
					if(Attackable()){ //need to actually define attackable or eliminate
						this.targetgridpos2D.x = i; 
						this.targetgridpos2D.y = j;
						MonoBehaviour.print ("Target acquired!");
					DirectionUpdate();
						//MonoBehaviour.print(i);
						//MonoBehaviour.print(j);
						return;
				}
					
					
					
				}
			}
		MonoBehaviour.print("A target was not acquired");
		}
		

	
	
	
	void DirectionUpdate(){
		this.targetdistancex = this.targetgridpos2D.x - this.gridpos2D.x;
		this.targetdistancey = this.targetgridpos2D.y - this.gridpos2D.y;
	}

	void Attack(){
		// this.targetgridpos2D.x, this.targetgridpos2D.y, this.damage
		}

	void PrintZombiePosition(){
		string pstring = "Zombie position: " + this.gridpos2D.x.ToString() + ',' + gridpos2D.y.ToString();
		MonoBehaviour.print(pstring);
		}

	void Move(){
		MonoBehaviour.print ("Zombie position before move");
		PrintZombiePosition ();

		
		
		if (Math.Abs (this.targetdistancex) + Math.Abs (this.targetdistancey) <= this.attackrange) {
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
			if(this.targetdistancex > 0){
				this.gridpos2D.x += 1;
			}
			
			else{
				this.gridpos2D.x -= 1;
			}
				
				
			}
			
		else{
				this.xmove = true;
			if(this.targetdistancey > 0){
				this.gridpos2D.y += 1;
			}
			
			else{
				this.gridpos2D.y -= 1;
			}
			
				this.DirectionUpdate();
			}
			
			
			
			
			
		}
		
	}
	

