using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour
{
    // Start is called before the first frame update
    Toggle toggle;
    void Start()
    {
        toggle = gameObject.GetComponent<Toggle>();
        string temp = gameObject.name;
        toggle.onValueChanged.AddListener(delegate {AttachCallback(temp);});
    }

    public void AttachCallback(string tgl)
    {
        if (tgl.CompareTo("MLToggle") == 0)
        {
            if(toggle.isOn)
            {
                GameMode.instance.usingML(true);
            } 
            else
            {
                GameMode.instance.usingML(false);
            }
        } 
        else if (tgl.CompareTo("ElemenToggle") == 0)
        {
            if(toggle.isOn)
            {
                GameMode.instance.usingElement(true);
            } 
            else
            {
                GameMode.instance.usingElement(false);
            }
        }
    }
}
