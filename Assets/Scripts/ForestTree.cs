using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTree : MonoBehaviour
{
    public VolcanoTile Tile { get; set; }

    public bool OnFire { get; set; } = false;

    public float Temperature = 0.0f;

    public void OnConsumedByLava()
    {
        if (!this.OnFire)
        {
            this.OnFire = true;
            this.Temperature += 100f;
        }
    }

    public void UpdateFire()
    {
        this.Temperature += 10.0f;

        if (this.Temperature >= 80.0f)
        {
            this.OnFire = true;
            this.GetComponentInChildren<Renderer>().material = this.Tile.Volcano.lavaMaterial;
        }

        if (this.Temperature > 200.0f)
        {
            this.ConsumeTree();
        }
    }

    private void ConsumeTree()
    {
        this.Tile.Tree = null;
        Destroy(this.gameObject);
    }
}
