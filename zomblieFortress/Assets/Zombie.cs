
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Zombie : MonoBehaviour {
	public Point gridpos2D;
	public Point oldgridpos2D;
	Point targetgridpos2D;
	int xmax = Board.widthx -1;
	int ymax = Board.widthy -1;
	int searchx;
	int searchy;
	int targetdistancex;
	int targetdistancey;
	float health;
	public int attackrange;
	public float attackdamage;
	bool xmove = true;
	public bool needtarget = true;
	public Board metaboard;


	public void init(Board board, Point Spawngridpos2D){
		this.attackrange = 1;
		this.attackdamage = 10f;
		this.health = 100f;
		this.metaboard = board;
		this.gridpos2D = Spawngridpos2D;
		this.oldgridpos2D = Spawngridpos2D;
		this.needtarget = true;
		this.UpdateZombieBoard();
	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		
	}

	public void TakeDamage(float damage) {
		Debug.Log ("ow " + health + " - " + damage);
		this.health -= damage;
		if (this.health < 0f) {
			MonoBehaviour.print("This zombie is dead");
			Board.gameBoard.boardzombie[this.gridpos2D.x,this.gridpos2D.y]= null;
			Board.gameBoard.zombies.Remove(this);
			Destroy(gameObject);
		}
		
	}


	void FindTargetClosest(){
		Point candgridpos2D = this.metaboard.walls[0].gridpos2D;
		int distance = xmax + ymax; 
		

		foreach (Wall w in this.metaboard.walls) {
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

		if (this.metaboard.boardwall [this.targetgridpos2D.x, this.targetgridpos2D.y] == null) {
						return false;
				}

		return true;
		}


	void FindTargetRandom(){
		int wallN = this.metaboard.walls.Count;

		int wallR = UnityEngine.Random.Range (0, wallN);
		this.targetgridpos2D.x = this.metaboard.walls [wallR].gridpos2D.x;
		this.targetgridpos2D.y = this.metaboard.walls [wallR].gridpos2D.y;
		MonoBehaviour.print ("Got random wall! " + wallR.ToString());



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

	public void UpdateZombieBoard(){
		this.metaboard.boardzombie [this.oldgridpos2D.x, this.oldgridpos2D.y] = null; //need to delete object?
		this.metaboard.boardzombie [this.gridpos2D.x, this.gridpos2D.y] = this;
	}

	
	void UpdateUnityPosition(){
		Vector3 pos = this.metaboard.gridPointToWorldPos (this.gridpos2D, 0);
		this.gameObject.transform.position = pos;
		}


	public void TakeTurn(){
		//MonoBehaviour.print ("zombie is taking a turn");
		//MonoBehaviour.print (needtarget);
		if (needtarget) {
			this.FindTargetRandom(); //this.FindTargetDumbLoop();
			needtarget = false; //eventually change
				}
		bool nonewmove = true;
		int failcount = 0;

		while (nonewmove & failcount < 5) {
				Move();
				if(this.gridpos2D == this.oldgridpos2D){
				failcount += 1;
				this.FindTargetRandom();
				}
				else{
				nonewmove = false;
			}

		}





		UpdateZombieBoard ();
		UpdateUnityPosition();
		}


	void DirectionUpdate(){
		this.targetdistancex = this.targetgridpos2D.x - this.gridpos2D.x;
		this.targetdistancey = this.targetgridpos2D.y - this.gridpos2D.y;
	}

	int DistanceToTarget(Point move){
		return Math.Abs(this.targetgridpos2D.x - move.x) + Math.Abs(this.targetgridpos2D.y - move.y);

		}

	void Attack(){

		//his.metaboard.boardwall[this.targetgridpos2D.x, this.targetgridpos2D.y].TakeDamage(this.attackdamage);
		// this.targetgridpos2D.x, this.targetgridpos2D.y, this.damage
		}

	void PrintZombiePosition(){
		string pstring = "Zombie position: " + this.gridpos2D.x.ToString() + ',' + this.gridpos2D.y.ToString();
		MonoBehaviour.print(pstring);
		}

	bool OnBoard(Point p){
		if (p.x < Board.widthx & p.y < Board.widthy & p.x >= 0 & p.y >= 0) {
			return true;
		}
		return false;

	}
	void WallNextDoor(){
		List<Point> nextdoor = CardGen.getCardinal (this.gridpos2D);
	
				foreach (Point p in nextdoor) {

						if (OnBoard (p)) {

								if (Board.gameBoard.boardwall [p.x, p.y] == null) {

				}
				else {

										this.targetgridpos2D = p;
										return;
								}
								
						}


				}

		}

	
	void Move(){
		WallNextDoor ();
		this.DirectionUpdate ();
		this.oldgridpos2D = this.gridpos2D;
				//MonoBehaviour.print("Distance to target: " + DistanceToTarget(this.gridpos2D).ToString() + " Attack range: " + this.attackrange.ToString());
				//MonoBehaviour.print (DistanceToTarget (this.gridpos2D) <= this.attackrange);
		        if (DistanceToTarget(this.gridpos2D) <= this.attackrange) {
						//MonoBehaviour.print("condition met for an attack");
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
				
				int count = 0;
				while (count < 5) {
						count += 1;
						for (int i = 0; i < cmoves.Count; i++) {
								Point temp = cmoves [i];
								int randomIndex = UnityEngine.Random.Range (i, cmoves.Count);
								cmoves [i] = cmoves [randomIndex];
								cmoves [randomIndex] = temp;
						}
				}
		

		
		
		foreach (Point p in cmoves) {
							
						cdistance = DistanceToTarget (p);
						if (p.x < Board.widthx & p.y < Board.widthy & p.x >= 0 & p.y >= 0) {
								if (cdistance < distance & this.metaboard.boardwall [p.x, p.y] == null & this.metaboard.boardzombie [p.x, p.y] == null) {
										distance = cdistance;
										move = p;
								}

						

						}

				}
				this.gridpos2D = move;
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


	