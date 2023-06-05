using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUIOnStart : MonoBehaviour
{
    public GameObject nameUI;
    public GameObject createUI;
    public GameObject chatroomUI;

    private void Awake()
    {
        nameUI.SetActive(true);
        createUI.SetActive(false);
        chatroomUI.SetActive(false);
    }
}
