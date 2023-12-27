using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangeFinder
{
    public List<OverlayTile> GetTilesInRange(OverlayTile startTile, int range, PathFinder pathFinder)
    {
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startTile);

        while(stepCount < range)
        {
            var surroundingTiles = new List<OverlayTile>();
            foreach (var tile in tileForPreviousStep)
            {
                surroundingTiles.AddRange(pathFinder.GetNeighbourTiles(tile, new List<OverlayTile>()));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }

    public List<OverlayTile> GetTilesInRangeStraight(OverlayTile startTile, int range)
    {
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 1;
        var map = MapManager.Instance.map;
        while(stepCount <= range)
        {
            Vector2Int stepDown = startTile.Position2D + new Vector2Int(0, 0-stepCount);
            Vector2Int stepUp = startTile.Position2D + new Vector2Int(0, stepCount);
            Vector2Int stepLeft = startTile.Position2D + new Vector2Int(0-stepCount, 0);
            Vector2Int stepRight = startTile.Position2D + new Vector2Int(stepCount, 0);
            if(map.ContainsKey(stepDown))
                inRangeTiles.Add(map[stepDown]);
            if(map.ContainsKey(stepUp))
                inRangeTiles.Add(map[stepUp]);
            if(map.ContainsKey(stepLeft))
                inRangeTiles.Add(map[stepLeft]);
            if(map.ContainsKey(stepRight))
                inRangeTiles.Add(map[stepRight]);
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }

}
