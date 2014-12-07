using System.Collections.Generic;

public class FarmCluster {
    public Point center;
    public int farms_contained;

    public FarmCluster (int center_x, int center_y, int num_contained) {
        this.center = new Point(center_x, center_y);
        this.farms_contained = num_contained;
    }

    public static List<FarmCluster> FindClusters (bool[,] known_farms) {
        int[,] found_clusters = new int[known_farms.GetLength(0), known_farms.GetLength(1)];


        // recurse through the matrix of known farms.  Detected clusters are
        // identified by an integer ID, unique to each cluster.  Start with 1, since
        // the array will be filled with 0 by default.
        int cluster_id = 1;

        for (int i = 0; i < found_clusters.GetLength(0); i++){
            for (int j = 0; j < found_clusters.GetLength(1); j++) {
                if (known_farms[i, j] == true &&found_clusters[i, j] != 0){
                    Point start_point = new Point(i, j);
                    RecursiveLocate(ref found_clusters, known_farms, start_point, cluster_id);
                    cluster_id++;
                }
            }
        }
        List<FarmCluster> cluster_list = new List<FarmCluster>();
        return cluster_list;
    }

    public static bool PointInBounds(Point point, int[,] matrix){
        if (point.x < 0 || point.x >= matrix.GetLength(0) || point.y < 0 || point.y >= matrix.GetLength(1)){
            return false;
        }
        return true;
    }

    private static void RecursiveLocate(ref int[,] found_clusters, bool[,] known_farms, Point start_point, int cluster_id){
        // base case: this cell is not a farm.
        if (known_farms[start_point.x, start_point.y] == false){
            return;
        }
        else {
            found_clusters[start_point.x, start_point.y] = cluster_id;
        }

        foreach (Point new_search_index in CardGen.getAdjacent(start_point)) {
            if (PointInBounds(new_search_index, found_clusters) == false) {
                RecursiveLocate(ref found_clusters, known_farms, new_search_index, cluster_id);
            }
        }
    }
}
