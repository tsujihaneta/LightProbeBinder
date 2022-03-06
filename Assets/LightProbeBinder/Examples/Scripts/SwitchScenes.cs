using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
		{
            SceneUtility.LoadSceneAsyncAdditive("StageA");
            SceneUtility.UnoadSceneAsync("StageB");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneUtility.UnoadSceneAsync("StageA");
            SceneUtility.LoadSceneAsyncAdditive("StageB");
        }
    }
}
