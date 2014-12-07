
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Zombie : MonoBehaviour {
	public Point gridpos2D;
	Point targetgridpos2D;
	int xmax = Board.widthx -1;
	int ymax = Board.widthy -1;
	int searchx;
	int searchy;
	int targetdistancex;
	int targetdistancey;
	int attackrange;
	bool xmove = true;
	bool needtarget = false;
	public Board metaboard;


	
	public Zombie (Point gridpos2D, int attackrange, Board gameboard){
		this.gridpos2D = gridpos2D;
		this.attackrange = attackrange;
		this.metaboard = metaboard;
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

		
	}


	void FindTargetClosest(){
		Point candgridpos2D = this.metaboard.walls[0].gridpos2D;
		int distance = xmax + ymax; 
		

		foreach (Wall w in metaboard.walls) {
			Point p = w.gridpos2D;


			int cdistance = Math.Abs(this.gridpos2D.x - p.x) + Math.Abs(this.gridpos2D.y - p.y);
						if (cdistance <= distance) {
							distance = cdistance;
							candgridpos2D = p;
						}
				}

		this.targetgridpos2D = candgridpos2D;
		this.targetgridpos2D.x = candgridpos2D.x;
		this.targetgridpos2D.y = candgridpos2D.y;
		DirectionUpdate();

	}

	bool Attackable (){

		if (metaboard.boardwall [this.targetgridpos2D.x, this.targetgridpos2D.y] == null) {
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
		

	void UpdateUnityPosition(){
		Vector3 pos = this.metaboard.gridPointToWorldPos (this.gridpos2D, 0);
		this.gameObject.transform.position = pos;
		}
	
	public void TakeTurn(){
		MonoBehaviour.print ("zombie is taking a turn");

		if (needtarget) {
			this.FindTargetDumbLoop();
			needtarget = true; //eventually change
				}
		Move();
		UpdateUnityPosition ();
		}

	void DirectionUpdate(){
		this.targetdistancex = this.targetgridpos2D.x - this.gridpos2D.x;
		this.targetdistancey = this.targetgridpos2D.y - this.gridpos2D.y;
	}

	int DistanceToTarget(Point move){
		return Math.Abs(this.targetgridpos2D.x - move.x) + Math.Abs(this.targetgridpos2D.y - move.y);

		}

	void Attack(){
		// this.targetgridpos2D.x, this.targetgridpos2D.y, this.damage
		}

	void PrintZombiePosition(){
		string pstring = "Zombie position: " + this.gridpos2D.x.ToString() + ',' + this.gridpos2D.y.ToString();
		MonoBehaviour.print(pstring);
		}



	void Move(){
				this.DirectionUpdate ();

				if (Math.Abs (this.targetdistancex) + Math.Abs (this.targetdistancey) <= this.attackrange) {
						this.Attack ();
						return;
				}
				Point move = new Point (this.gridpos2D.x, this.gridpos2D.y);
				int distance = DistanceToTarget (this.gridpos2D);
				int cdistance;



				Point up = new Point (this.gridpos2D.x, this.gridpos2D.y + 1);
				Point down = new Point (this.gridpos2D.x, this.gridpos2D.y - 1);
				Point right = new Point (this.gridpos2D.x + 1, this.gridpos2D.y);
				Point left = new Point (this.gridpos2D.x - 1, this.gridpos2D.y);
		
				List<Point> cmoves = new List<Point> ();
				cmoves.Add (up);
				cmoves.Add (down);
				cmoves.Add (right);
				cmoves.Add (left);

				foreach (Point p in cmoves) {
						cdistance = DistanceToTarget (p);
						if (cdistance < distance) {
								distance = cdistance;
								move = p;
						}

						this.gridpos2D = move;

				}
		DirectionUpdate ();
		}


	void OldMove(){
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
						if (this.targetdistancex > 0) {
								this.gridpos2D.x += 1;
						} else {
								this.gridpos2D.x -= 1;
						}
				
				
				} else {
						if (this.xmove == false & this.gridpos2D.y != this.targetgridpos2D.y) {
								this.xmove = true;
								if (this.targetdistancey > 0) {
										this.gridpos2D.y += 1;
								} else {
										this.gridpos2D.y -= 1;
								}
			
								
						}
				}
			
			
			
		this.DirectionUpdate ();

			
		}
		
	}
	

