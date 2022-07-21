using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScene : MonoBehaviour
{
    
    public string customSceneName;
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        LoadSceneByName(customSceneName);
        Debug.Log("on button clicked");
    }
    private void LoadSceneByName(string sceneName)
    {
    }
    
   
}
