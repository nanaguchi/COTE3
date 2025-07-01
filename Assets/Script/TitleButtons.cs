using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene("Sample Scene");
    }
}