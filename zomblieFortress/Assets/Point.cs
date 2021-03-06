public struct Point {
	public int x;
	public int y;

	public static Point operator +(Point p1, Point p2){
		return new Point (p1.x + p2.x, p1.y + p2.y);
	}

	public static bool operator ==(Point p1, Point p2){
		return p1.x == p2.x && p1.y == p2.y;
	}

	public static bool operator !=(Point p1, Point p2){
		return !(p1.x == p2.x && p1.y == p2.y);
	}

	public static int ManhattanDistance(Point p1, Point p2){
		return System.Math.Abs(p1.x-p2.x) + System.Math.Abs(p1.y-p2.y);
	}

	public Point (int x, int y){
		this.x = x;
		this.y = y;
	}
}