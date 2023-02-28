using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Controles(){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);

    }

    public void Salida(){
        
        Application.Quit();
    }

    public void volver(){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);

    }
    public void volveramenu(){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-2);

    }
}
