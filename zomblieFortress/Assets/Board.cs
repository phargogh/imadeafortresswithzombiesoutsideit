using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public GameObject wallFab;
	public GameObject towerFab;
	public GameObject farmFab;
	public Hand hand;
	public static int widthx = 32;
	public static int widthy = 32;
	public Wall [,] boardwall = new Wall[widthx,widthy];
	public Zombie [,] boardzombie = new Zombie[widthx,widthy];
	public List<GameObject> farms = new List<GameObject>();
	public List<Zombie> zombies = new List<Zombie>();
	public List<Wall> walls = new List<Wall>();
	public List<Tower> towers = new List<Tower>();
	public List<Point> zombiegridpos2D = new List<Point>();
	public List<Point> wallgridpos2D = new List<Point>();
    public List<FarmCluster> farmclusters = new List<FarmCluster>();

	public Stack<GameObject> shadowSquares = new Stack<GameObject> ();
	public Stack<GameObject> inactiveShadowSquares = new Stack<GameObject> ();

	public Stack<GameObject> shadowTowers = new Stack<GameObject> ();
	public Stack<GameObject> inactiveShadowTowers = new Stack<GameObject> ();

	private long last_update;
	private long last_cash_update;
	private bool trigger_farm_detection ;

	private List<Point> adjacency_mask = new List<Point>(){
		new Point(0,-1),
		new Point(-1,0),
		new Point(0,1),
		new Point(1,0),
		new Point(-1,-1),
		new Point(-1,1),
		new Point(1,-1),
		new Point(1,1),
	};
	
	// Use this for initialization
	void Start () {
		Point center = new Point (16, 16);
		List<Point> start_walls = CardGen.getAdjacent (center);


		List<Point> corners = new List<Point> (){
			new Point (0, 0),
			new Point (0, 31),
			new Point (31, 0),
			new Point (31, 31),
		};
		
		spawnWalls (start_walls, start_walls, new Point());
		spawnWalls (corners,corners, new Point());
        FindFarmClusters();
	}

    void FindFarmClusters(){
		bool[,] known_farms = Farm.DetectFarmland(this);
        List<FarmCluster> detected_clusters = FarmCluster.FindClusters(known_farms);
        this.farmclusters = detected_clusters;

    }
	
	// Update is called once per frame
	void Update () {
		long num_ticks = DateTime.Now.Ticks;
		long current_time = num_ticks / 10000;  // time in ms
		if (current_time >= this.last_update + 1000 || this.trigger_farm_detection == true) {
            FindFarmClusters();
			this.last_update = current_time;
			this.trigger_farm_detection = false;  // reset so we don't re-detect farmland
		}

        if (current_time >= this.last_cash_update + 5000) {
            hand.add_resources(this.farms.Count);
            this.last_cash_update = current_time;
        }

		if (hand.selected) {
			Point gridPos =  worldPosToGridPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			SetShadowSquares(hand.selected.walls, hand.selected.towers, gridPos);
		} else {
			SetShadowSquares(new List<Point>(), new List<Point>(), new Point());
		}
	}

	void SetShadowSquares(List<Point> walls, List<Point> towers, Point gridPos){
		while(shadowSquares.Count > 0) {
			GameObject g = shadowSquares.Pop();
			g.SetActive(false);
			inactiveShadowSquares.Push(g);
		}
		foreach(Point w in walls) {
			int x = gridPos.x + w.x;
			int y = gridPos.y + w.y;
			GameObject g = inactiveShadowSquares.Count > 0 ? inactiveShadowSquares.Pop() : (GameObject) Instantiate(wallFab, new Vector3(), Quaternion.identity);
			g.SetActive(true);
			g.transform.position = gridPointToWorldPos(new Point(x, y), -1);
			Color c = (x >= 1 && x < widthx-1 && y >= 1 && y < widthy-1 && boardwall[x,y] == null) ? (boardzombie[x,y] == null ? Color.white : Color.magenta) : Color.red;
			g.GetComponent<SpriteRenderer>().color = c;
			shadowSquares.Push(g);
		}

		while(shadowTowers.Count > 0) {
			GameObject g = shadowTowers.Pop();
			g.SetActive(false);
			inactiveShadowTowers.Push(g);
		}
		foreach(Point t in towers) {
			int x = gridPos.x + t.x;
			int y = gridPos.y + t.y;
			GameObject g = inactiveShadowTowers.Count > 0 ? inactiveShadowTowers.Pop() : (GameObject) Instantiate(towerFab, new Vector3(), Quaternion.identity);
			g.SetActive(true);
			g.transform.position = gridPointToWorldPos(new Point(x, y), -2);
			shadowTowers.Push(g);
		}

	}

	public Point worldPosToGridPoint(Vector3 worldPos) {
		Point gridPos = new Point ();
		gridPos.x = Mathf.RoundToInt(worldPos.x - transform.position.x) + widthx/2;
		gridPos.y = Mathf.RoundToInt(worldPos.y - transform.position.y) + widthy/2;
		return gridPos;
	}

	public Vector3 gridPointToWorldPos(Point gridPoint, float z) {
		Vector3 worldPos = new Vector3 ();
		worldPos.x = transform.position.x + gridPoint.x - widthx / 2;
		worldPos.y = transform.position.y + gridPoint.y - widthy / 2;
		worldPos.z = z;
		return worldPos;
	}

	public bool spawnWalls(List<Point> walls, List<Point> towers, Point gridPos){
		//Debug.Log("placing walls near: " + gridPos.x + ", " + gridPos.y + " like " + walls[0].x);
		List<Point> wallsToPlace = new List<Point> ();
		foreach (Point w in walls) {
			Point p = new Point(gridPos.x + w.x, gridPos.y + w.y);
			if (p.x >= 1 && p.x < widthx-1 && p.y >= 1 && p.y < widthy-1 && boardwall[p.x,p.y] == null){
				wallsToPlace.Add(p);
			}
			else{
				return false;
			}
		}
		foreach (Point p in wallsToPlace) {
			placeWall(p);
		}
		this.trigger_farm_detection = true;  // trigger farmland to be re-detected.
		return true;
	}

	void placeWall(Point p){
		Vector3 pos = new Vector3(p.x - Board.widthx/2, p.y - Board.widthy/2, 0);
		GameObject wall = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
		boardwall [p.x, p.y] = wall.GetComponent<Wall>();
		this.walls.Add(wall.GetComponent<Wall>());
		wall.GetComponent<Wall> ().gridpos2D = p;
	}

    void OnGUI() {
        // write the resources to the screen.
        int label_x_pos = Screen.width/2 + 200;
        int label_y_pos = Screen.height/2 - 200;
        int row_y_offset = 20;

        string label_string = "Resources: ";
        GUI.Label(new Rect(label_x_pos, label_y_pos, 150, 150), label_string + hand.get_resources().ToString());

        string farm_count_label = "Active farms: ";
        GUI.Label(new Rect(label_x_pos, label_y_pos + row_y_offset, 150, 150), farm_count_label + this.farms.Count.ToString());

        string farm_cluster_label = "Active farm clusters: ";
        GUI.Label(new Rect(label_x_pos, label_y_pos + 40, 150, 150), farm_cluster_label + this.farmclusters.Count.ToString());

        //string farm_cluster_label = "Farms clustered: ";
        //GUI.Label(new Rect(label_x_pos, label_y_pos + 40, 150, 150), this.farmclusters[0]+ this.farmclusters[0].farms_contained.ToString());


        int offset = 3;
        foreach (FarmCluster farm_cluster in this.farmclusters) {
            GUI.Label(new Rect(label_x_pos, label_y_pos + row_y_offset*offset , 150, 150), farm_cluster.farms_contained.ToString());
            offset++;
        }

    }
}
