using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class CardGen{

	// Use this for initialization
	public static List<Point> createShape(int n) {
		if (n <= 1) {
			return new List<Point> (){new Point(0,0)};
		} 
		else {
			List<Point> shape = createShape(n-1);
			// choose random point in shape 
			int index = Math.Floor(Random.Range(0,shape.Count));
			Point p = shape[index];

			//then choose a random unoccupied adjacent point and place point

		}
	
	}

	public static List<Point> getAdjacent(Point p){
		List<Point> adjacency_mask = new List<Point>(){
			new Point(0,-1),
			new Point(-1,0),
			new Point(0,1),
			new Point(1,0),
			new Point(-1,-1),
			new Point(-1,1),
			new Point(1,-1),
			new Point(1,1),
		};
		List<Point> adjacent_points = new List<Point>();
		foreach (Point a in adjacency_mask){
			adjacent_points.Add(p+a);
		}
		return adjacent_points;
	}

}
