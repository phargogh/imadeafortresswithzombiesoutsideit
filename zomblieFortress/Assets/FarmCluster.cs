using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FarmCluster {
    public Point center;
    public int farms_contained;

    public FarmCluster (int center_x, int center_y, int num_contained) {
        this.center = new Point(center_x, center_y);
        this.farms_contained = num_contained;
    }

    public FarmCluster (List<Point> contained_farms) {
        this.center = CenterPoint(contained_farms);
        this.farms_contained = contained_farms.Count;
    }

    public static Point CenterPoint(List<Point> point_list) {
        return point_list[0];  // TODO: calculate the centerpoint.
    }

    public static Point ToPixelDims(Point target_point) {
        int label_x_pos = Screen.width/2 + target_point.x;
        int label_y_pos = Screen.height/2 - target_point.y;

        return new Point(label_x_pos, label_y_pos);
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

        Hashtable cluster_hash = new Hashtable();
        for (int i = 0; i < found_clusters.GetLength(0); i++) {
            for (int j = 0; j < found_clusters.GetLength(1); j++) {
                int found_id = found_clusters[i, j];
                Point found_point = new Point(i, j);
                if (cluster_hash.ContainsKey(found_id)) {
                    List<Point> cluster_points = (List<Point>) cluster_hash[found_id];
                    cluster_points.Add(found_point);
                    cluster_hash.Remove(found_id);
                    cluster_hash.Add(found_id, cluster_points);
                }
                else {
                    List<Point> cluster_list = new List<Point>();
                    cluster_hash.Add(found_id, cluster_list);
                }
            }
        }

        List<FarmCluster> final_cluster_list = new List<FarmCluster>();
        foreach (DictionaryEntry dict in cluster_hash){
            // dict.key is the cluster ID
            // dict.value is a list of points belonging to the cluster.
            FarmCluster farm_cluster = new FarmCluster((List<Point>) dict.Value);
            final_cluster_list.Add(farm_cluster);
        }
        return final_cluster_list;
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
