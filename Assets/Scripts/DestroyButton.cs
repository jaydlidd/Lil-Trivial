using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyButton : MonoBehaviour
{

    public GameObject thisButton;
    public Button thisBut;
    private GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        
        thisBut.onClick.AddListener(TaskToDo);
    }

    void TaskToDo()
    {
        int index = transform.GetSiblingIndex();
        gm.GetComponent<GameManager>().RemovePlayer(index);

        Destroy(thisButton);
    }
}
