using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchStages : MonoBehaviour
{
    GameObject stageA = null;
    GameObject stageB = null;

    // Start is called before the first frame update
    void Start()
    {
        stageA = GameObject.Find("StageA");
        stageB = GameObject.Find("StageB");

        if (stageA != null)
        {
            stageA.SetActive(false);

        }
        if (stageB != null)
        {
            stageB.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stageA == null)
        {
            stageA = GameObject.Find("StageA");
        }

        if (stageB == null)
        {
            stageB = GameObject.Find("StageB");
        }

        if (stageA != null && stageB != null)
        {
            if (stageA.activeSelf && stageB.activeSelf)
            {
                stageA.SetActive(true);
                stageB.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
		{
            if (stageA != null)
            {
                stageA.SetActive(false);

            }
            if (stageB != null)
            {
                stageB.SetActive(false);
            }

            if (stageA != null)
			{
                stageA.SetActive(true);
			}
            if (stageB != null)
            {
                stageB.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (stageA != null)
            {
                stageA.SetActive(false);

            }
            if (stageB != null)
            {
                stageB.SetActive(false);
            }

            if (stageA != null)
            {
                stageA.SetActive(false);
            }
            if (stageB != null)
            {
                stageB.SetActive(true);
            }
        }
    }
}
