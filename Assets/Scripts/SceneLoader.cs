using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    } 
}
