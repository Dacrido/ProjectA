using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding { // A* Algorithm

    private Grid<PathNode> grid;
    private List<PathNode> openNodes;
    private HashSet<PathNode> closedNodes;
    public Pathfinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 10f, Vector2.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    private List<PathNode> FindPath (int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startY, startX);

        openNodes = new List<PathNode>() { startNode };
        closedNodes = new HashSet<PathNode>();


    }



}
