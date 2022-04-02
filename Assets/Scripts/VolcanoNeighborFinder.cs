using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoNeighborFinder
{
    public static void PopulateLowerNeighborsForTiles(Volcano volcano, IEnumerable<VolcanoTile> tiles)
    {
        foreach (VolcanoTile tile in tiles)
        {
            List<VolcanoTile> neighbors = new List<VolcanoTile>();
            
            if (volcano.GetTile(tile.TileX - 1, tile.TileY) is VolcanoTile lowerLeft)
            {
                neighbors.Add(lowerLeft);
            }

            /*if (volcano.GetTile(tile.TileX - 1, tile.TileY - 1) is VolcanoTile lowerBottom)
            {
                neighbors.Add(lowerBottom);
            }*/

            if (volcano.GetTile(tile.TileX, tile.TileY - 1) is VolcanoTile lowerRight)
            {
                neighbors.Add(lowerRight);
            }

            tile.LowerOrEqualNeighbors = neighbors;
        }
    }
}
