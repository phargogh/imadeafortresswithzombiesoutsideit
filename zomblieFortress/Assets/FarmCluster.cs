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

    // Get the centroid of the bounding box, returned as a point.
    public static Point CenterPoint(List<Point> point_list) {
        if (point_list.Count == 0) {
            return new Point(0, 0);
        }

        int minx = point_list[0].x;
        int maxx = point_list[0].x;
        int miny = point_list[0].y;
        int maxy = point_list[0].y;

        foreach (Point point in point_list) {
            if (point.x <= minx) {
                minx = point.x;
            }
            if (point.x >= maxx) {
                maxx = point.x;
            }
            if (point.y <= miny) {
                miny = point.y;
            }
            if (point.y >= maxy) {
                maxy = point.y;
            }
        }

        int x_center = minx + maxx / 2;
        int y_center = miny + maxy / 2;

        return new Point(x_center, y_center);
    }

    public static Point ToPixelDims(Point target_point) {
        int board_centerx = Board.widthx / 2;
        int board_centery = Board.widthy / 2;

        int scale = 5;



        int label_x_pos = (Screen.width/2 + target_point.x - board_centerx) * scale;
        int label_y_pos = (Screen.height/2 - target_point.y - board_centery) * scale;

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
                if (known_farms[i, j] == true && found_clusters[i, j] == 0){
                    Point start_point = new Point(i, j);
                    RecursiveLocate(ref found_clusters, known_farms, start_point, cluster_id);
                    cluster_id++;
                }
            }
        }

        Hashtable cluster_hash = new Hashtable();
        for (int i = 0; i < found_clusters.GetLength(0); i++) {
            for (int j = 0; j < found_clusters.GetLength(1); j++) {
                if (found_clusters[i, j] != 0) {
                    int found_id = found_clusters[i, j];
                    Point found_point = new Point(i, j);
                    if (cluster_hash.ContainsKey(found_id)) {
                        List<Point> cluster_points = (List<Point>) cluster_hash[found_id];
                        cluster_points.Add(found_point);
                        cluster_hash[found_id] = cluster_points;
                    }
                    else {
                        List<Point> cluster_list = new List<Point>();
                        cluster_list.Add(found_point);
                        cluster_hash.Add(found_id, cluster_list);
                    }
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
        if (known_farms[start_point.x, start_point.y] == false){
            return;  // base case: this cell is not a known farm
        }
        else if (found_clusters[start_point.x, start_point.y] != 0) {
            return;  // base case: this cell has already been visited
        }
        else {
            // otherwise, this is a new farm to add to the cluster.
            found_clusters[start_point.x, start_point.y] = cluster_id;
        }

        foreach (Point new_search_index in CardGen.getAdjacent(start_point)) {
            if (PointInBounds(new_search_index, found_clusters) == true) {
                RecursiveLocate(ref found_clusters, known_farms, new_search_index, cluster_id);
            }
        }
    }
}
