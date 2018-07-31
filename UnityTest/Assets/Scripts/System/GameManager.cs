using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager> {

	public void ToTitle()
    {
        SceneManager.LoadScene("Title Scene");
    }

    public void ToGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}
