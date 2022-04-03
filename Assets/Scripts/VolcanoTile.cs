using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoTile : MonoBehaviour
{
    private float lavaLevel;

    public int TileX { get; set; }

    public int TileY { get; set; }

    public float Depth { get; set; }

    public float Height { get; set; }

    public float LavaLevel
    {
        get => this.lavaLevel;

        set
        {
            if (this.lavaLevel != value)
            {
                this.lavaLevel = Mathf.Clamp(value, 0.0f, 5.0f); //Mathf.Min(value, 1.0f);
                this.UpdateLavaLevel();
            }

            //this.lavaLevel = Mathf.Clamp(value, 0.0f, 2.0f); //Mathf.Min(value, 1.0f);
            //this.UpdateLavaLevel();
        }
    }

    public float LevelWithLava
    {
        get
        {
            return (1 - this.Depth) + this.LavaLevel;//this.LavaLevel;
        }
    }

    public float LavaFlow { get; set; } = 0.0f;

    public Volcano Volcano { get; set; }

    public List<VolcanoTile> LowerOrEqualNeighbors = new List<VolcanoTile>();

    public List<VolcanoTile> TreeNeighbors = new List<VolcanoTile>();

    public ScoreResource ScoreResource { get; set; }

    public ForestTree Tree;

    public bool Dig(float amount)
    {
        if (this.LavaLevel > 0f)
        {
            return false;
        }

        if (this.Depth >= 1.0f)
        {
            return false;
        }
        else
        {
            this.Depth += amount;
            if (this.Depth > 1.0f)
            {
                this.Depth = 1.0f;
            }
            this.UpdateLavaLevel();
            return true;
        }
    }

    private void Start()
    {
        this.UpdateLavaLevel();
    }

    private void UpdateLavaLevel()
    {
        if (this.lavaLevel > 0)
        {
            this.gameObject.GetComponent<Renderer>().material = this.Volcano.lavaMaterial;

            this.ScoreResource?.OnLavaConsumed();
            this.ScoreResource = null;

            this.Tree?.OnConsumedByLava();

            //TODO: Set tree on fire
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material = this.Volcano.volcanoMaterial;
        }

        float newZ = -Mathf.Clamp(this.LevelWithLava, 0.0f, 2.0f);//this.Depth - this.LavaLevel;

        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, newZ);
    }

    /*private void OnMouseDown()
    {
        this.Depth = 1.0f;
        this.UpdateLavaLevel();
    }*/
}
