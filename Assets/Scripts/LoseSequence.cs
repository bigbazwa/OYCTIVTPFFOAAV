using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseSequence : MonoBehaviour
{
    private bool inProgress = false;

    public Transform LoseLava;

    public float LoseLavaSpeed = 10f;

    public float LoseLavaStopHeight = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (this.inProgress && this.LoseLava.position.y < this.LoseLavaStopHeight)
        {
            this.LoseLava.Translate(Vector3.up * Time.deltaTime * this.LoseLavaSpeed, Space.World);
        }
    }

    public void Begin()
    {
        StartCoroutine(this.LoseSequenceCoroutine());
    }

    private IEnumerator LoseSequenceCoroutine()
    {
        Debug.Log("Begin Lose Sequence");
        this.inProgress = true;
        yield return new WaitForSeconds(5.0f);
        Debug.Log("Begin End Sequence");

        SceneManager.LoadScene(0);
    }
}
