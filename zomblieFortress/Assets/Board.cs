using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public static Board gameBoard;

	public GameObject wallFab;
	public GameObject towerFab;
	public GameObject farmFab;
	public Hand hand;
	public static int widthx = 32;
	public static int widthy = 32;
	public List<Point> borderList = new List<Point> ();
	public List<Point> borderListLeft = new List<Point> ();
	public List<Point> borderListRight = new List<Point> ();
	public List<Point> borderListTop = new List<Point> ();
	public List<Point> borderListBottom = new List<Point> ();

	public Wall [,] boardwall = new Wall[widthx,widthy];
	public Zombie [,] boardzombie = new Zombie[widthx,widthy];
	public List<GameObject> farms = new List<GameObject>();
	public List<Zombie> zombies = new List<Zombie>();
	public List<Wall> walls = new List<Wall>();

    public List<FarmCluster> farmclusters = new List<FarmCluster>();

	public Stack<GameObject> shadowSquares = new Stack<GameObject> ();
	public Stack<GameObject> inactiveShadowSquares = new Stack<GameObject> ();

	public Stack<GameObject> shadowTowers = new Stack<GameObject> ();
	public Stack<GameObject> inactiveShadowTowers = new Stack<GameObject> ();

	private long last_update;
	private long last_cash_update;
	private bool trigger_farm_detection ;


	// Use this for initialization
	void Start () {
		Point center = new Point (16, 16);
		List<Point> start_walls = CardGen.getAdjacent (center);
		List<Point> start_towers = CardGen.getCardinal (center);

		spawnWalls (start_walls, start_towers, new Point());
		BorderPoints();
		SideBorder ();

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
		
//		foreach (Wall w in walls) {
//			if (boardwall[w.gridpos2D.x,w.gridpos2D.y] == null) {
//				w.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
//				//Debug.LogError("***null case*** Wall in walls not at boardwall " + w.gridpos2D.x + ", " + w.gridpos2D.y);
//				//Debug.LogError("Wall in walls not boardwall", w.gameObject);
//			} 
//			else if (boardwall[w.gridpos2D.x,w.gridpos2D.y] != w) {
//				w.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
//				//boardwall[w.gridpos2D.x,w.gridpos2D.y].gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
//				//Debug.LogError("Wall in walls not at boardwall " + w.gridpos2D.x + ", " + w.gridpos2D.y);
//				//if (boardwall[w.gridpos2D.x,w.gridpos2D.y].gridpos2D != w.gridpos2D) {
//				//	boardwall[w.gridpos2D.x,w.gridpos2D.y].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
//				//}
//			}
//			else {
//				w.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
//			}
//		}
//		for (int i = 0; i < widthx; i++) {
//			for (int j = 0; j < widthy; j++) {
//				Wall w = boardwall[i,j];
//				//if(w && (w.gridpos2D.x != i || w.gridpos2D.y != j)) {
//				if(w && !walls.Contains(w)) {
//					w.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
//				}
//			}
//		}
		Board.gameBoard = this;
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
			g.transform.position = gridPointToWorldPos(new Point(x, y), -0.5f);
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
			g.transform.position = gridPointToWorldPos(new Point(x, y), -0.6f);
			shadowTowers.Push(g);
		}

	}

	public Point worldPosToGridPoint(Vector3 worldPos) {
		Point gridPos = new Point ();
		gridPos.x = Mathf.RoundToInt(worldPos.x - transform.position.x) + widthx/2;
		gridPos.y = Mathf.RoundToInt(worldPos.y - transform.position.y) + widthy/2;
		return gridPos;
	}

	public Vector3 gridPointToWorldPos(Point gridPoint, float zOffest) {
		Vector3 worldPos = new Vector3 ();
		worldPos.x = transform.position.x + gridPoint.x - widthx / 2;
		worldPos.y = transform.position.y + gridPoint.y - widthy / 2;
		worldPos.z = transform.position.z - widthy + gridPoint.y + zOffest;
		return worldPos;
	}

	public bool spawnWalls(List<Point> walls, List<Point> towers, Point gridPos){

//		foreach (Point w in walls) {
//			int count = 0;
//			foreach (Point p in walls) {
//				if (p == w) count++;
//			}
//			if (count != 1) {
//				Debug.LogError("*** Wrong number " + count + " of duplicate wall in spawnwalls");
//			}
//		}
//		foreach (Point t in towers) {
//			int count = 0;
//			foreach (Point p in walls) {
//				if (p == t) count++;
//			}
//			if (count != 1) {
//				Debug.LogError("*** Wrong number " + count + " of walls for this tower in spawnwalls");
//			}
//		}

		//Debug.Log("placing walls near: " + gridPos.x + ", " + gridPos.y + " like " + walls[0].x);
		List<Point> wallsToPlace = new List<Point> ();
		foreach (Point w in walls) {
			Point p = new Point(gridPos.x + w.x, gridPos.y + w.y);
			if (p.x >= 1 && p.x < widthx-1 && p.y >= 1 && p.y < widthy-1 && boardwall[p.x,p.y] == null){
				wallsToPlace.Add(p);
			} else {
				return false;
			}
		}
		foreach (Point p in wallsToPlace) {
			placeWall(p);
		}
		foreach (Point t in towers) {
			Point towerGridPos = new Point(gridPos.x + t.x, gridPos.y + t.y);
			Vector3 pos = gridPointToWorldPos(towerGridPos, -0.2f);
			GameObject towerObj = (GameObject) Instantiate(towerFab, pos, Quaternion.identity);
			Tower tower = towerObj.GetComponent<Tower>();
			boardwall[towerGridPos.x, towerGridPos.y].SetTower(tower);
		}
		this.trigger_farm_detection = true;  // trigger farmland to be re-detected.
		return true;
	}

	void placeWall(Point p){
		if (this.boardwall [p.x, p.y] != null) {
			Debug.LogError("Already a wall at " + p.x + ", " + p.y + "  cannot place");
			return;
		}

		Vector3 pos = gridPointToWorldPos (p, 0);
		GameObject wallObj = (GameObject) Instantiate(wallFab, pos, Quaternion.identity);
		Wall wall = wallObj.GetComponent<Wall>();
		this.boardwall [p.x, p.y] = wall;
		this.walls.Add(wall);
		wall.gridpos2D = p;
	}

	public void BorderPoints(){
		List<Point> plist = new List<Point> ();

		for (int i = 0; i < widthx; i++) {
			plist.Add (new Point(i,0));
			plist.Add (new Point(i, widthy - 1));
		}

		for (int i = 1; i < widthy - 1; i++) {
			plist.Add (new Point(0,i));
			plist.Add (new Point(widthx - 1,i));
		}

		this.borderList = plist;
	}

	public void SideBorder(){
		List<Point> plist1 = new List<Point> ();
		
		for (int i = 0; i < widthx; i++) {
			plist1.Add (new Point(i,0));
		}
		
		this.borderListBottom = plist1;
		
		List<Point> plist2 = new List<Point> ();
		
		for (int i = 0; i < widthx; i++) {
			plist2.Add (new Point(i, widthy - 1));
		}
		this.borderListTop = plist2;

		List<Point> plist3 = new List<Point> ();
		
		for (int i = 1; i <  widthy; i++) {
			plist3.Add (new Point(0,i));
		}
		this.borderListLeft = plist3;
		List<Point> plist4 = new List<Point> ();
		
		for (int i = 1; i < widthy; i++) {
			plist4.Add (new Point(widthx - 1,i));
		}
		
		this.borderListRight = plist4;

	}

    public static int CellSize() {
        // returns a reasonably close approximation of the length of the side of one game piece (in pixels)
        int min_screen_size;
        int board_size;
        if (Screen.width > Screen.height) {
            min_screen_size = Screen.height;
            board_size = Board.widthx;
        }
        else {
            min_screen_size = Screen.width;
            board_size = Board.widthy;
        }
        return min_screen_size / board_size;
    }

    public Point ScreenCenter() {
        return new Point(Screen.width/2, Screen.height/2);
    }

    void OnGUI() {
        // write the resources to the screen.
        int label_x_pos = Screen.width/2 + 200;
        int label_y_pos = Screen.height/2 - 200;
        int row_y_offset = 20;

        int cell_size = CellSize();
        Point screen_center = ScreenCenter();
        int stats_x = screen_center.x + cell_size * Board.widthx/2;
        int stats_y = screen_center.y - cell_size * Board.widthy/2;

        string label_string = "Resources: ";
        GUI.Label(new Rect(stats_x, stats_y, 150, 150), label_string + hand.get_resources().ToString());

        string farm_count_label = "Active farms: ";
        GUI.Label(new Rect(stats_x, stats_y + row_y_offset, 150, 150), farm_count_label + this.farms.Count.ToString());

        string farm_cluster_label = "Active farm clusters: ";
        GUI.Label(new Rect(stats_x, stats_y + 40, 150, 150), farm_cluster_label + this.farmclusters.Count.ToString());

        int offset = 3;
//        foreach (FarmCluster farm_cluster in this.farmclusters) {
//            GUI.Label(new Rect(stats_x, stats_y + row_y_offset * offset , 150, 150), farm_cluster.farms_contained.ToString());
//            Point cluster_center = FarmCluster.ToPixelDims(farm_cluster.center);
//            GUI.Label(new Rect(cluster_center.x, cluster_center.y , 20, 20), farm_cluster.farms_contained.ToString());
//            offset++;
//        }

    }
}
