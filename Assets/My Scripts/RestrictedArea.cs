using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    public string warningMessage = "Restricted Area!";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_Canvas.instance.ShowWarning(warningMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_Canvas.instance.HideWarning();
        }
    }
}