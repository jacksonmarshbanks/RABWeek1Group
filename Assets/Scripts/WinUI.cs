using TMPro;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text finalTimeText;

    void Start()
    {
        var pc = PlayerController.instance;
        if (pc != null && pc.hasFinalTime && finalTimeText != null)
        {
            finalTimeText.text = "Final Time: " + pc.finalTimeDisplay;
        }
        else if (finalTimeText != null)
        {
            finalTimeText.text = "Final Time: --:--";
        }
    }
}
