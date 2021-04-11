using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public int nextSceneLoad;
    public int subSceneLoad;
    public int level;
    private int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        subSceneLoad = SceneManager.GetActiveScene().buildIndex;
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        level = PlayerPrefs.GetInt("levelReached", 1);
    }
    public void Continue ()
    {
        if (currentLevel < level)
        {
            if(currentLevel%3 == 0)
            {
                currentLevel += 1;
                PlayerPrefs.SetInt("currentLevel", currentLevel);
                SceneManager.LoadScene(nextSceneLoad);
            }
            else
            {
                currentLevel += 1;
                PlayerPrefs.SetInt("currentLevel", currentLevel);
                SceneManager.LoadScene(subSceneLoad);
            }
        }
        else
        {
            
            if (level == 15)
            {
                Debug.Log("YOU WIN GAME");
                PlayerPrefs.DeleteAll();
            }
            else
            {
                if (level % 3 == 0)
                {
                    SceneManager.LoadScene(nextSceneLoad);
                    level += 1;
                    currentLevel += 1;
                    PlayerPrefs.SetInt("currentLevel", currentLevel);
                    Debug.Log(level);
                }
                else
                {
                    SceneManager.LoadScene(subSceneLoad);
                    level += 1;
                    currentLevel += 1;
                    PlayerPrefs.SetInt("currentLevel", currentLevel);
                    Debug.Log(level);
                }


                if (level > PlayerPrefs.GetInt("levelReached"))
                {
                    PlayerPrefs.SetInt("levelReached", level);
                }
            }
        }
    }

    public void Quit ()
    {
        SceneManager.LoadScene("Level_select");
    }
}
