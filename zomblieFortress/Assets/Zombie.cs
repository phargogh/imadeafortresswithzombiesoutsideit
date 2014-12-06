using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	int x;
	int y;
	int targetx;
	int targety;
	int xmax = getboardx ();
	int ymax = getboardy ();
	int searchx;
	int searchy;
	int directionx;
	int directiony;
	bool targetacquired
	bool xmove;

	Zombie (int x, int y){
		this.x = x;
		this.y = y;
		this.FindTarget();
		this.xmove = true; //randomize
		}
	
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FindTarget(){

		bool targetacquired = false;

		for (int i = 1; i <= this.xmax; i++){
		
			if (self.targetacquired){
				break;
			}
			else{
				for (int j = 1; j <= this.ymax; j++){
					if(attackable(i,j)){
						this.targetx = i;
							this.targety = j;
						this.targetacquired = true;
						break;
						}

					}
				}
			}
		this.DirectionUpdate()				
		}


	void DirectionUpdate(){
		this.directionx = this.targetx - this.x;
		this.directiony = this.targety - this.y;
	}
	void Move(){
		if (this.x == this.targetx){
			this.xmove = false 



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
