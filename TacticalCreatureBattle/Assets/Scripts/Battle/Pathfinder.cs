using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    readonly GridWalls _gridWalls;
    readonly RectInt _levelBoundary;

    public Pathfinder(GridWalls gridWalls)
    {
        _gridWalls = gridWalls;
        _levelBoundary = _gridWalls.GetBoundaries(4);
        if (_levelBoundary.width == 0 || _levelBoundary.height == 0)
        {
            Debug.LogError($"{nameof(Pathfinder)}: Level boundary is undefined.");
        }
    }

    public bool CanMove(Vector3Int cell, Direction direction)
    {
        return _gridWalls.CanMove(cell, direction);
    }

    public bool CanFall(Vector3Int cell, Direction direction)
    {
        return _gridWalls.CanFall(cell, direction);
    }

    public bool UnitCanMove(UnitController unit, Direction direction)
    {
        if (unit.UnitSize == Size.Small || unit.UnitSize == Size.Medium)
        {
            return CanMove(unit.Position, direction);
        }
        Vector3Int cell1 = unit.Position;
        Vector3Int cell2 = unit.Position;
        switch (direction)
        {
            case Direction.Left:
                cell2 += Vector3Int.up;
                break;
            case Direction.Down:
                cell2 += Vector3Int.right;
                break;
            case Direction.Right:
                cell1 += Vector3Int.right;
                cell2 += Vector3Int.up + Vector3Int.right;
                break;
            case Direction.Up:
                cell1 += Vector3Int.up;
                cell2 += Vector3Int.up + Vector3Int.right;
                break;
        }
        return CanMove(cell1, direction) && CanMove(cell2, direction);
    }

    public bool UnitCanFall(UnitController unit, Direction direction)
    {
        if (unit.UnitSize == Size.Small || unit.UnitSize == Size.Medium)
        {
            return CanFall(unit.Position, direction);
        }
        Vector3Int cell1 = unit.Position;
        Vector3Int cell2 = unit.Position;
        switch (direction)
        {
            case Direction.Left:
                cell2 += Vector3Int.up;
                break;
            case Direction.Down:
                cell2 += Vector3Int.right;
                break;
            case Direction.Right:
                cell1 += Vector3Int.right;
                cell2 += Vector3Int.up + Vector3Int.right;
                break;
            case Direction.Up:
                cell1 += Vector3Int.up;
                cell2 += Vector3Int.up + Vector3Int.right;
                break;
        }
        return CanFall(cell1, direction) && CanFall(cell2, direction);
    }

    public Vector2Int[] GetCellsWithinRange(UnitController unit, int range, bool includeUnitSpace)
    {
        return GetCellsWithinRange(unit.Position.x, unit.Position.y, unit.UnitSize, range, includeUnitSpace);
    }

    public Vector2Int[] GetCellsWithinRange(int x, int y, Size size, int range, bool includeUnitSpace)
    {
        if (range < 0 || (range == 0 && !includeUnitSpace))
        {
            return Array.Empty<Vector2Int>();
        }
        // Calculate each cell's distance from the source, up to the range.
        int[,] distances = CalculateDistances(x, y, size, range);
        // If not including the unit space, override the calculated distances.
        if (!includeUnitSpace)
        {
            for (int i = 0; i < (size == Size.Large ? 2 : 1); i++)
            {
                for (int j = 0; j < (size == Size.Large ? 2 : 1); j++)
                {
                    distances[x - _levelBoundary.xMin + i, y - _levelBoundary.yMin + j] = int.MaxValue;
                }
            }
        }
        // Return the collection of cell coordinates in range.
        List<Vector2Int> coords = new List<Vector2Int>();
        for (int i = 0; i < distances.GetLength(0); i++)
        {
            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (distances[i, j] <= range)
                {
                    coords.Add(new Vector2Int(i + _levelBoundary.xMin, j + _levelBoundary.yMin));
                }
            }
        }
        return coords.ToArray();
    }

    int[,] CalculateDistances(int x, int y, Size size, int maxRange)
    {
        int[,] distances = new int[_levelBoundary.width, _levelBoundary.height];
        distances.Fill(int.MaxValue);
        SearchNode[,] nodes = SearchNode.CreateNodeArray(_levelBoundary.width, _levelBoundary.height);
        List<SearchNode> unvisited = SearchNode.ListNodes(nodes);
        // Initialize the source cells.
        int length = size == Size.Large ? 2 : 1;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                int sourceX = x - _levelBoundary.xMin + i;
                int sourceY = y - _levelBoundary.yMin + j;
                nodes[sourceX, sourceY].BestDistanceFromSource = 0;
                distances[sourceX, sourceY] = 0;
            }
        }
        // Dijkstra's algorithm
        while (unvisited.Count > 0)
        {
            unvisited.Sort();
            SearchNode currentNode = unvisited[0];
            if (currentNode.BestDistanceFromSource >= maxRange)
            {
                break;
            }

            // TODO: Consider all of currentNode's unvisited neighbors and update their tentative distances.

            unvisited.Remove(currentNode);
        }
        return distances;
    }

    class SearchNode : IComparable<SearchNode>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int BestDistanceFromSource { get; set; }

        public SearchNode(int x, int y)
        {
            X = x;
            Y = y;
            BestDistanceFromSource = int.MaxValue;
        }

        public static SearchNode[,] CreateNodeArray(int width, int height)
        {
            SearchNode[,] nodes = new SearchNode[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new SearchNode(x, y);
                }
            }
            return nodes;
        }

        public static List<SearchNode> ListNodes(SearchNode[,] nodes)
        {
            List<SearchNode> nodeList = new List<SearchNode>();
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    nodeList.Add(nodes[x, y]);
                }
            }
            return nodeList;
        }

        public int CompareTo(SearchNode other)
        {
            if (BestDistanceFromSource < other.BestDistanceFromSource)
            {
                return -1;
            }
            else if (BestDistanceFromSource == other.BestDistanceFromSource)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
