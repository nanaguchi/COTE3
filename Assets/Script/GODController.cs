using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GODController : MonoBehaviour
{
    // Inspector‚©‚çİ’è‚·‚é€–Ú
    [SerializeField] private ToggleGroup toggleGroup; 
    [SerializeField] private string sceneNameToLoad;  

    void Update()
    {
        if (toggleGroup.AnyTogglesOn())
        {
            // EnterƒL[‚ª‰Ÿ‚³‚ê‚½‚©
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("mars");
            }
        }
    }
}