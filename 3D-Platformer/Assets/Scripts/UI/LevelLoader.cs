using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1.0f;
    private void Update()
    {
        //Condition to change level!!
        //if (Input.GetMouseButtonDown(2))
        //{
        //    LoadNextLevel();
        //}
    }

    public void LoadNextLevel()
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));

    }

    IEnumerator loadLevel(int lvlIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(lvlIndex);
    }

    public void DemoButtonStart()
    {
        LoadNextLevel();
    }


}