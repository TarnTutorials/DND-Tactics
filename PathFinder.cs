using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();
        
        openList.Add(start);
        while(openList.Count > 0)
        {
            OverlayTile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if(currentTile == end)
            {
                return GetFinishedList(start, end);
            }
            var neighbours = GetNeighbourTiles(currentTile, searchableTiles); 
            foreach(var neighbour in neighbours)
            {
                if(closedList.Contains(neighbour))
                {
                    continue;
                }
                neighbour.G = GetDistance(start, neighbour);
                neighbour.H = GetDistance(end, neighbour);

                neighbour.PreviousTile = currentTile;
                if(!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }
        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.PreviousTile;
        }
        return finishedList;
    }

    private int GetDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.Position.x - neighbour.Position.x) + Mathf.Abs(start.Position.y - neighbour.Position.y);
    }

    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentTile, List<OverlayTile> searchableTiles)
    {
        Dictionary<Vector2Int, OverlayTile> tileToSearch = new Dictionary<Vector2Int, OverlayTile>();
        var map = MapManager.Instance.map;
        if(searchableTiles.Count > 0)
        {
            foreach (var tile in searchableTiles)
            {
                tileToSearch.Add(tile.Position2D, tile);
            }
        }
        else
        {
            tileToSearch = map;
        }

        List<OverlayTile> neighbours = new List<OverlayTile>();
        foreach(var direction in Direction2D.directionList)
        {
            Vector2Int locationToCheck = new Vector2Int(currentTile.Position.x + direction.x,currentTile.Position.y + direction.y);
            if(tileToSearch.ContainsKey(locationToCheck))
            {
                if(!tileToSearch[locationToCheck].isBlocked)
                {
                    neighbours.Add(tileToSearch[locationToCheck]);
                }
            }
        }
        return neighbours;
    }
}
