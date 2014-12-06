using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public GameObject tilePrefab;
	static int width = 32;
	GameObject [,] board = new GameObject[width,width];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < width; j++) {
				//board[i,j] = GameObject.CreatePrimitive(PrimitiveType.
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
