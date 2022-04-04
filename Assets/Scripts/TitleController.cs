using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public Text level1hs;

    public Text level2hs;

    public Text level3hs;

    // Start is called before the first frame update
    void Start()
    {
        level1hs.text = $"{PlayerPrefs.GetInt("HSLevel1", 0)}";

        level2hs.text = $"{PlayerPrefs.GetInt("HSLevel2", 0)}";

        level3hs.text = $"{PlayerPrefs.GetInt("HSLevel3", 0)}";
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.pause = !AudioListener.pause;
        }
    }

    public void OnNormalClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnHardClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void OnRandomClicked()
    {
        SceneManager.LoadScene(3);
    }
}
