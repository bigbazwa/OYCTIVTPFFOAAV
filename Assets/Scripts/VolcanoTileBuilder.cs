using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VolcanoTileBuilder : MonoBehaviour
{
    public Transform sidePlane;

    public GameObject volcanoTilePrefab;

    public GameObject scoreTilePrefab;

    public GameObject treeTilePrefab;

    public int height = 25;

    public float perlinNoiseScale = 21.4f;

    public float forestPerlinNoiseScale = 3.52f;

    public float forestThreshold = 0.6f;

    public Vector2[] scoreTiles;

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
                float z = (Mathf.PerlinNoise(i * perlinNoiseScale, j * perlinNoiseScale) / 2.5f) - 1.0f;
                newTileObject.GetComponent<Transform>().Translate(new Vector3(i, j, z), Space.Self);
                
                newTileObject.name = $"Volcano tile i: {i},\tj: {j},\theight: {newTileObject.transform.position.y}";

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

                /*if (this.scoreTiles.Any(scoreTile => scoreTile.x == i && scoreTile.y == j))
                {
                    GameObject scoreTile = Instantiate(scoreTilePrefab, newTile.transform.position + Vector3.up * 2.0f, Quaternion.identity);

                    newTile.ScoreResource = scoreTile.GetComponent<ScoreResource>();
                }*/

                if (Mathf.PerlinNoise(i * forestPerlinNoiseScale, j * forestPerlinNoiseScale) >= forestThreshold + (newTile.Height + 11.0f) / 160.0f)
                {
                    GameObject treeTile = Instantiate(treeTilePrefab, newTile.transform.position + Vector3.back + Vector3.up * 1.0f, Quaternion.identity);
                    treeTile.transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f));
                    newTile.Tree = treeTile.GetComponent<ForestTree>();
                    newTile.Tree.Tile = newTile;
                }

                tiles.Add(newTile);
            }
        }

        return tiles;
    }
}
