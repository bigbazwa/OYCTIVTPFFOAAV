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

    public static void PopulateTreeNeighborsForTiles(Volcano volcano, IEnumerable<VolcanoTile> tiles)
    {
        foreach (VolcanoTile tile in tiles)
        {
            List<VolcanoTile> neighbors = new List<VolcanoTile>();

            if (volcano.GetTile(tile.TileX - 1, tile.TileY) is VolcanoTile lowerLeft && lowerLeft.Tree != null)
            {
                neighbors.Add(lowerLeft);
            }

            if (volcano.GetTile(tile.TileX - 1, tile.TileY - 1) is VolcanoTile lowerBottom && lowerBottom.Tree != null)
            {
                neighbors.Add(lowerBottom);
            }

            if (volcano.GetTile(tile.TileX, tile.TileY - 1) is VolcanoTile lowerRight && lowerRight.Tree != null)
            {
                neighbors.Add(lowerRight);
            }

            if (volcano.GetTile(tile.TileX + 1, tile.TileY) is VolcanoTile upperLeft && upperLeft.Tree != null)
            {
                neighbors.Add(upperLeft);
            }

            if (volcano.GetTile(tile.TileX + 1, tile.TileY + 1) is VolcanoTile upperTop && upperTop.Tree != null)
            {
                neighbors.Add(upperTop);
            }

            if (volcano.GetTile(tile.TileX, tile.TileY + 1) is VolcanoTile upperRight && upperRight.Tree != null)
            {
                neighbors.Add(upperRight);
            }

            tile.TreeNeighbors = neighbors;
        }
    }
}
