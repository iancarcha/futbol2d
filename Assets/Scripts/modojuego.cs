using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class modojuego : MonoBehaviour
{
    public void pvp(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
    }

   public void pvia(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+4);
    }

    public void iavsia(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+5);
    }
}
