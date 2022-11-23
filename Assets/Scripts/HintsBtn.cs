using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsBtn : MonoBehaviour
{

    public void OnButtonPress(){
        var guide = GameObject.FindObjectOfType<Guide>();
        if (guide)
        {
            guide.hintsButton();
        }
    }
}
