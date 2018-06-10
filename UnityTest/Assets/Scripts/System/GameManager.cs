using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager> {

	public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
