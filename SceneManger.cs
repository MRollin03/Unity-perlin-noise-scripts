using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    // Scenemanager scriptet søre for vores 
    // navigation mellem eksemplerne
    // funktioner kaldes af UI knapper.

    public void Menu(){
        SceneManager.LoadScene("Menu"); // loader scene med
                                        // givende navn.
    }

    public void LightScene(){
        SceneManager.LoadScene("lightscene");
    }

    public void HeigthMapScene(){
        SceneManager.LoadScene("Island");
    }

    public void TextureScene(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit(){
        Application.Quit(); // slukker for applikationen.
    }
}
