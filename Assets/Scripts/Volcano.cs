using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VolcanoTileBuilder))]
public class Volcano : MonoBehaviour
{
    public string LevelId = "Level";

    public Material volcanoMaterial;

    public Material lavaMaterial;

    public float lavaUpdateInterval = 1.0f;

    public float lavaFlow = 0.75f;

    public GameObject boulderPrefab;

    public float[] boulderTimes;

    public float[] boulderAngles;

    private int boulderIndex;

    private VolcanoTileBuilder tileBuilder;

    private List<VolcanoTile> tiles;

    private float lavaUpdateTimer = 0.0f;

    public int score = 0;

    public Text scoreText;

    public ParticleSystem treeSmoke;

    // Start is called before the first frame update
    void Start()
    {
        this.tileBuilder = GetComponent<VolcanoTileBuilder>();
        this.tiles = this.tileBuilder.BuildTiles();
        this.tiles.ForEach(tile => tile.Volcano = this);
        VolcanoNeighborFinder.PopulateLowerNeighborsForTiles(this, this.tiles);
        VolcanoNeighborFinder.PopulateTreeNeighborsForTiles(this, this.tiles);

        StartCoroutine(this.BoulderCoroutine());
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

                if (tile.Tree != null)
                {
                    if (Random.Range(0.0f, 1.0f) > 0.75f)
                    {
                        this.treeSmoke.transform.position = tile.Tree.transform.position;
                        this.treeSmoke.Play();
                    }
                }
                

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
        }

        this.scoreText.text = $"{this.score}";

        return anyScoreResourceLeft;
    }

    private IEnumerator BoulderCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.boulderTimes[this.boulderIndex]);

            GameObject boulder = Instantiate(this.boulderPrefab, new Vector3(-0.11f, 25.98f, 71.33f), Quaternion.identity);

            boulder.GetComponent<Rigidbody>().AddForce(new Vector3(this.boulderAngles[this.boulderIndex] / 25.0f, 0.5f, 0.0f), ForceMode.Impulse);

            Destroy(boulder, 15.0f);

            if (++this.boulderIndex >= this.boulderTimes.Length)
            {
                this.boulderIndex = 0;
            }
        }
    }
}
