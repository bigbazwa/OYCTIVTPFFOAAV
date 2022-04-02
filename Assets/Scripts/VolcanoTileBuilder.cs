using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoTileBuilder : MonoBehaviour
{
    public Transform sidePlane;

    public GameObject volcanoTilePrefab;

    public int height = 25;

    public float perlinNoiseScale = 21.4f;

    public List<VolcanoTile> BuildTiles()
    {
        List<VolcanoTile> tiles = new List<VolcanoTile>();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i + j < height)
                {
                    continue;
                }

                GameObject newTileObject = Instantiate(volcanoTilePrefab, sidePlane, false);
                float z = Mathf.PerlinNoise(i * perlinNoiseScale, j * perlinNoiseScale);
                newTileObject.GetComponent<Transform>().Translate(new Vector3(i, j, z), Space.Self);
                newTileObject.name = $"Volcano tile i: {i},\tj: {j},\tdepth: {z}";

                VolcanoTile newTile = newTileObject.GetComponent<VolcanoTile>();
                newTile.TileX = i;
                newTile.TileY = j;
                newTile.Depth = z;
                newTile.Height = newTileObject.transform.position.y;

                if (i == height - 1 && j == height - 1)
                {
                    // This is the top tile.
                    //newTile.LavaFlow = 0.5f;
                    newTile.LavaFlow = 50.0f;
                }

                tiles.Add(newTile);
            }
        }

        return tiles;
    }
}
