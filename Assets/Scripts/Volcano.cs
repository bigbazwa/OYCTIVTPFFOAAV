using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(VolcanoTileBuilder))]
public class Volcano : MonoBehaviour
{
    public Material volcanoMaterial;

    public Material lavaMaterial;

    public float lavaUpdateInterval = 1.0f;

    public float lavaFlow = 0.75f;

    private VolcanoTileBuilder tileBuilder;

    private List<VolcanoTile> tiles;

    private float lavaUpdateTimer = 0.0f;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.tileBuilder = GetComponent<VolcanoTileBuilder>();
        this.tiles = this.tileBuilder.BuildTiles();
        this.tiles.ForEach(tile => tile.Volcano = this);
        VolcanoNeighborFinder.PopulateLowerNeighborsForTiles(this, this.tiles);
        VolcanoNeighborFinder.PopulateTreeNeighborsForTiles(this, this.tiles);
    }

    // Update is called once per frame
    void Update()
    {
        this.lavaUpdateTimer += Time.deltaTime;
        if (this.lavaUpdateTimer >= this.lavaUpdateInterval)
        {
            this.lavaUpdateTimer = 0.0f;

            if (!this.UpdateLavaFlow())
            {
                GameObject.FindObjectOfType<LoseSequence>()?.Begin();
                this.lavaUpdateInterval /= 4f;
            }
        }
    }

    public VolcanoTile GetTile(int x, int y)
    {
        return this.tiles.FirstOrDefault(tile => tile.TileX == x && tile.TileY == y);
    }

    public bool Dig(Vector3 position, float range, float amount)
    {
        List<VolcanoTile> tilesInRange = new List<VolcanoTile>();

        foreach (VolcanoTile tile in this.tiles)
        {
            float distance = Vector3.Distance(position, tile.transform.position);

            if (distance <= range)
            {
                tilesInRange.Add(tile);
            }
        }

        bool successfulDig = false;

        tilesInRange.ForEach(tile => successfulDig |= tile.Dig(amount));

        return successfulDig;
    }

    private bool UpdateLavaFlow()
    {
        bool anyScoreResourceLeft = false; //TODO: start with false. should check remaining unburned trees
        List<VolcanoTile> lowerNeighbors = new List<VolcanoTile>();

        foreach (VolcanoTile tile in this.tiles)
        {
            tile.LavaLevel += tile.LavaFlow;

            if (tile.ScoreResource != null)
            {
                anyScoreResourceLeft = true;
            }

            if (tile.Tree?.OnFire == true)
            {
                tile.Tree.UpdateFire();

                foreach (VolcanoTile treeNeighbor in tile.TreeNeighbors)
                {
                    treeNeighbor.Tree?.UpdateFire();
                }
            }

            if (tile.Tree != null)
            {
                this.score += 1; //TODO: move to seperate method that runs at different frequency
                anyScoreResourceLeft = true;
            }

            if (tile.LavaLevel <= 0.0f)
            {
                continue;
            }

            lowerNeighbors.Clear();

            VolcanoTile lowest = null;

            foreach (VolcanoTile lowerNeighbor in tile.LowerOrEqualNeighbors)
            {
                if (lowerNeighbor.LevelWithLava <= tile.LevelWithLava/* + this.lavaFlow*/)
                {
                    if (lowest == null || lowerNeighbor.LevelWithLava < lowest.LevelWithLava)
                    {
                        lowest = lowerNeighbor;
                    }

                    lowerNeighbors.Add(lowerNeighbor);
                }
            }

            //float lavaToFlow = Mathf.Min(this.lavaFlow, tile.LavaLevel);

            //lowerNeighbors.ForEach(neighbor => neighbor.LavaLevel += lavaToFlow / lowerNeighbors.Count);

            if (lowest != null)
            {
                float overflow = tile.LevelWithLava - lowest.LevelWithLava;


                //float lavaToFlow = tile.LavaLevel * 0.25f;

                float lavaToFlow = overflow * this.lavaFlow;

                tile.LavaLevel -= lavaToFlow;

                lowest.LavaLevel += lavaToFlow;
            }

            //lowerNeighbors.ForEach(neighbor => neighbor.LavaFlow = tile.LavaFlow / 2.0f);

            // Remember, higher depth = lower.

            /*tile.LavaLevel += tile.LavaFlow;

            lowerNeighbors.Clear();

            VolcanoTile lowestNeighbor = tile.LowerOrEqualNeighbors.FirstOrDefault();

            foreach (VolcanoTile lowerNeighbor in tile.LowerOrEqualNeighbors)
            {
                // Find neighboring lower tiles that have a lower depth than the current tiles depth plus its lava level.
                if (lowerNeighbor.Depth < tile.Depth + tile.LavaLevel)
                {
                    lowerNeighbors.Add(lowerNeighbor);
                }

                if (lowerNeighbor.Depth < lowestNeighbor?.Depth)
                {
                    lowestNeighbor = lowerNeighbor;
                }
            }

            float lavaToFlow = tile.LavaLevel * this.lavaFlow;

            tile.LavaLevel -= lavaToFlow;

            //lowerNeighbors.ForEach(neighbor => neighbor.LavaLevel += lavaToFlow / lowerNeighbors.Count);

            if (lowestNeighbor != null)
            {
                lowestNeighbor.LavaLevel += lavaFlow;
            }*/
        }

        Debug.Log($"Score: {this.score}");

        return anyScoreResourceLeft;
    }
}
