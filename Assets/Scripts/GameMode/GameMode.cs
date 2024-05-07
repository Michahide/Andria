using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool isUsingMLAgent;
    public bool isUsingElement;
    public static GameMode instance;

    public void Awake()
    {
        isUsingElement = true;
        isUsingMLAgent = true;
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void usingML(bool isUsingML)
    {
        if (isUsingML)
        {
            isUsingMLAgent = true;
        }
        else
        {
            isUsingMLAgent = false;
        }
    }

    public void usingElement(bool isElement)
    {
        if (isElement)
        {
            isUsingElement = true;
        }
        else
        {
            isUsingElement = false;
        }
    }
}
