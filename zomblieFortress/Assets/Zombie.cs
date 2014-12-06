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

	Zombie (int x, int y){
		this.x = x;
		this.y = y;
		this.FindTarget();
		}
	
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FindTargetLoop(){

		bool targetacquired = false;

		for (int i = 1; i <= this.xmax; i++)
		{
		
			if (targetacquired)
			{
				break;
			}
			else
			{
				for (int j = 1; j <= this.ymax; j++)
				{
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
