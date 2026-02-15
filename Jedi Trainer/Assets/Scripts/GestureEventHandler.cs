using MiVRy;
using UnityEngine;

public class GestureEventHandler : MonoBehaviour
{
    public void OnGestureCompleted(GestureCompletionData gestureCompletionData)
    {
        Debug.Log("TRYING TO DO GESTURE, SIMILARITY: " + gestureCompletionData.similarity);
        if(gestureCompletionData.gestureID < 0)
        {
            string errorMessage = GestureRecognition.getErrorMessage(gestureCompletionData.gestureID);
            return;
        }

        if(gestureCompletionData.similarity >= 0.25f)
        {
            Debug.Log("TRIGGERING GESTURE" + gestureCompletionData.gestureName);
            //ELECTRICITY GESTURE

            //HEALING GESTURE
        }
    }
}
