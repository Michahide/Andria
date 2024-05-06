using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static bool isUsingMLAgent;
    public static bool isUsingElement;
    private static GameMode instance;

    public void Awake()
    {
        isUsingElement = true;
        isUsingMLAgent = true;
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void usingML(bool isUsingML)
    {
        if (instance == null)
        {
            instance = this;
        }
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
        if (instance == null)
        {
            instance = this;
        }
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
