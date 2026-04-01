using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    public string warningMessage = "Restricted Area!";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UI_Canvas.instance.ShowWarning(warningMessage);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UI_Canvas.instance.HideWarning();
        }
    }
}