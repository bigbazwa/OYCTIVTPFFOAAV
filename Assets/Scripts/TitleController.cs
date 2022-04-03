using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public Text level1hs;

    // Start is called before the first frame update
    void Start()
    {
        level1hs.text = $"{PlayerPrefs.GetInt("HSLevel1", 0)}";
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButton("Fire1") || Input.GetButton("Jump") || Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(1);
        }*/

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnNormalClicked()
    {
        SceneManager.LoadScene(1);
    }
}
