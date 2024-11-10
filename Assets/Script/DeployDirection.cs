using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeployDirection : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 startDragPosition;
    private Vector2 dragDirection;
    private bool isOperatorPlaced = false; // Track if the operator has been placed

    public DeployHandler deployHandler;
    public Image directionArrow;
    public RectTransform diamondBoundary;
    public GameObject panelDirection;
    public Button cancelButton; // Reference to the cancel button
    private BuildManager buildManager; // Reference to BuildManager
    private Tower associatedTower; // The tower associated with this deployment

    // Threshold to determine if the analog is centered (small value to allow some leeway)
    private float centerThreshold = 60f;

    void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
        cancelButton.onClick.AddListener(CancelDeployment); // Set up the cancel button
        ResetAnalog(); // Ensure the analog starts in the center
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isOperatorPlaced)
        {
            startDragPosition = eventData.position;

            // Snap the arrow immediately to the nearest cardinal direction
            Vector2 currentDragPosition = eventData.position;
            dragDirection = currentDragPosition - startDragPosition;

            // Snap the direction to the closest cardinal direction
            Vector2 snappedDirection = SnapToCardinalArrow(dragDirection);

            // Update UI and arrow position immediately
            ShowDirectionFeedback(snappedDirection);

            // Set the direction of the attack range immediately
            deployHandler.UpdateDirectionInRealTime(snappedDirection);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isOperatorPlaced)
        {
            Vector2 currentDragPosition = eventData.position;
            dragDirection = currentDragPosition - startDragPosition;

            // Snap the drag direction to the nearest cardinal direction
            Vector2 snappedDirection = SnapToCardinalDirection(dragDirection);
            Vector2 analogDirection = SnapToCardinalArrow(dragDirection);
            // Show feedback for the current drag
            ShowDirectionFeedback(analogDirection);

            // Update attack range and rotate the operator in real-time based on snapped cardinal direction
            deployHandler.UpdateDirectionInRealTime(snappedDirection);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isOperatorPlaced)
        {
            // Snap the drag direction to one of the four cardinal directions
            Vector2 finalDragDirection = SnapToCardinalDirection(dragDirection);

            // If the analog is near the center, treat it as still waiting for input
            if (dragDirection.magnitude < centerThreshold)
            {
                ResetAnalog();
                return; // Don't lock in the direction if close to center
            }

            // Operator placement is done, now enter direction selection mode
            isOperatorPlaced = true;
            SetDirection(finalDragDirection); // Call SetDirection with the snapped cardinal direction
            HideDirectionFeedback();
        }
    }

    // Call this after the operator is placed and the direction is selected
    public void SetDirection(Vector2 selectedDirection)
    {
        deployHandler.ChooseDirection(selectedDirection);
        HideDirectionFeedback();

        // Reset the operator placed status so the next operator can be placed
        isOperatorPlaced = false;
    }

    // Snap to one of the four cardinal directions based on the drag vector
    private Vector2 SnapToCardinalDirection(Vector2 dragVector)
    {
        if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
        {
            return dragVector.x > 0 ? Vector2.up : Vector2.down;
        }
        else
        {
            return dragVector.y > 0 ? Vector2.left : Vector2.right;
        }
    }

    private Vector2 SnapToCardinalArrow(Vector2 dragVector)
    {
        if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
        {
            return dragVector.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return dragVector.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    private void ShowDirectionFeedback(Vector2 snappedDirection)
    {
        deployHandler.gameObject.SetActive(true);
        panelDirection.gameObject.SetActive(true);

        // Calculate the correct angle for the snapped direction (adjust to fix the offset)
        float angle = Mathf.Atan2(snappedDirection.y, snappedDirection.x) * Mathf.Rad2Deg;

        // Adjust the angle to align the arrow correctly (depending on how your UI is set up, you might need to subtract 90 degrees)
        angle -= 90f;  // Adjust by 90 degrees to align with the cardinal directions

        // Set the arrow to point in the snapped direction with the adjusted angle
        directionArrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        // Snap the arrow to the cardinal direction in UI (move the arrow within the boundary)
        directionArrow.rectTransform.anchoredPosition = snappedDirection * (diamondBoundary.rect.width / 2);
    }


    private void HideDirectionFeedback()
    {
        panelDirection.gameObject.SetActive(false);
        ResetAnalog();
    }

    private void ResetAnalog()
    {
        dragDirection = Vector2.zero;
        directionArrow.rectTransform.anchoredPosition = Vector2.zero; // Reset arrow position
    }

    public void SetAssociatedTower(Tower tower)
    {
        associatedTower = tower;
    }

    public void CancelDeployment()
    {
        // Check if buildManager is null
        if (buildManager == null)
        {
            Debug.LogError("CancelDeployment: buildManager is null. Make sure it's being assigned in Start.");
        }
        else
        {
            Debug.Log("CancelDeployment: buildManager is assigned correctly.");
        }

        // Check if associatedTower is null
        if (associatedTower == null)
        {
            Debug.LogError("CancelDeployment: associatedTower is null. Make sure it's set using SetAssociatedTower.");
        }
        else
        {
            Debug.Log("CancelDeployment: associatedTower is assigned correctly.");
        }

        // Recall the operator if both references are valid
        if (buildManager != null && associatedTower != null)
        {
            buildManager.RecallOperator(associatedTower);
            Debug.Log("CancelDeployment: Operator recall successful.");
        }
        else
        {
            Debug.Log("CancelDeployment: Something went wrong - cannot recall operator.");
        }

        // Reset deployment UI and status
        isOperatorPlaced = false; // Reset the operator placement status
        
        ResetAnalog(); // Reset the analog position and UI
        HideDirectionFeedback(); // Hide the direction feedback UI
        Time.timeScale = buildManager != null ? buildManager.GetDefaultTimeScale() : 1f; // Default to normal speed if buildManager is null
    }

}
