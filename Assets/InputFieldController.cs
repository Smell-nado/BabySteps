using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour {

    public Text inputText;
    public Text placeholderText;


    public void EnterName()
    {
        placeholderText.text = "Enter your name";
    }

    public void NameDone()
    {
        Debug.Log(inputText.text);
        //Fill wherever my array of player data is with the name
    }
}
