using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public void OnClickQuitButton()
    {
        print("Quit button was clicked");
        Application.Quit();
    }
}
