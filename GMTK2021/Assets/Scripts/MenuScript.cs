using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string InitialLevel = null;
    public void StartGame(){
        GameManager.Instance.StartLevelLoad(InitialLevel);
        //SceneManager.LoadScene(InitialLevel);
    }

    public void Quit(){
        Application.Quit();
    }
}
