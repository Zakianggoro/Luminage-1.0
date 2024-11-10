using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<CharacterBio> OnCharacterSelected;
    public static event Action OnDeploymentCompleted; // Add this new event

    public static void CharacterSelected(CharacterBio character)
    {
        Debug.Log("Event triggered for character: " + character.OperatorName);
        OnCharacterSelected?.Invoke(character);
    }

    public static void DeploymentCompleted()
    {
        Debug.Log("Deployment is completed.");
        OnDeploymentCompleted?.Invoke(); // Trigger this when deployment is finalized
    }

    public static event Action OnBackgroundClick;

    public static void BackgroundClicked()
    {
        OnBackgroundClick?.Invoke();
    }
}
