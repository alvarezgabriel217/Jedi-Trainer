using MiVRy;
using UnityEngine;

public class GestureEventHandler : MonoBehaviour
{
    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        Debug.Log("TRYING TO DO GESTURE, SIMILARITY: " + gestureCompletionData.similarity);
        if (gestureCompletionData.gestureID < 0)
        {
            string errorMessage = GestureRecognition.getErrorMessage(gestureCompletionData.gestureID);
            return;
        }

        if (gestureCompletionData.similarity >= 0.25f)
        {
            if (gestureCompletionData.gestureName == "Electricity")
            {
                if (GameManager.instance.player.GetComponent<Player>().usingElectricity) // TURN OFF ELECTRICITY
                {
                    GameManager.instance.player.GetComponent<Player>().StopLightning();
                }
                else // TURN ON ELECTRICITY
                {
                    GameManager.instance.player.GetComponent<Player>().ShootLightning();
                }
            }
            else if (gestureCompletionData.gestureName == "Healing")
            {
                GameManager.instance.player.GetComponent<Player>().Heal(50);
            }
            else if(gestureCompletionData.gestureName == "Future")
            {
                StartCoroutine(GameManager.instance.player.GetComponent<Player>().SeeFuture());
            }
            else if (gestureCompletionData.gestureName == "Dualwield")
            {
                GameManager.instance.player.GetComponent<Player>().DualWield();
            }
            Debug.Log("TRIGGERING GESTURE" + gestureCompletionData.gestureName);
        }
    }
}
