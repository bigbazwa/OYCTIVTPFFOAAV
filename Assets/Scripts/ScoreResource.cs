using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreResource : MonoBehaviour
{
    public int ScoreAmount = 100;

    public GameObject RescueePrefab;

    public float ScoreFrequency = 5.0f;

    public bool EmitRescuees = false;

    private float scoreTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.scoreTimer += Time.deltaTime;

        if (this.scoreTimer >= this.ScoreFrequency)
        {
            //TODO: event
            Debug.Log($"Scored: {this.ScoreAmount}");
            this.scoreTimer = 0.0f;

            if (this.EmitRescuees)
            {
                Instantiate(RescueePrefab, this.transform.position - Vector3.forward, Quaternion.identity);
            }
        }
    }

    public void OnLavaConsumed()
    {
        Destroy(this.gameObject);
    }
}
