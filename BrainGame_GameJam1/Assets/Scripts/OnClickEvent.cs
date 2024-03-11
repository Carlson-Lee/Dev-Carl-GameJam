using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OnClickEvent : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log ("You have clicked the button");
    }

}
