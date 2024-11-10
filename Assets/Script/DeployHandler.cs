using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployHandler : MonoBehaviour
{
    [Header("Deployment Settings")]
    [SerializeField] private Color directionHighlightColor; // Color for direction phase
    [SerializeField] private LayerMask tileLayerMask;
    [SerializeField] private GameObject directionUI; // UI element to show during dragging

    private List<Renderer> highlightedTiles = new List<Renderer>();
    private Plot currentPlot;
    private bool isChoosingDirection = false;
    private CharacterBio currentOperator;
    private Collider2D[] tilesInRange;
    private Color originalColor;

    private Vector2 previousDirection;

    private void Start()
    {
        previousDirection = Vector2.zero;
        directionUI.SetActive(false); // Ensure the direction UI is hidden initially
    }

    public void UpdateDirectionInRealTime(Vector2 direction)
    {
        if (currentOperator != null && direction != previousDirection)
        {
            Debug.Log("Updating direction in real-time: " + direction);
            RotateOperatorSprite(direction);
            UpdateAttackRangePoint(currentOperator, direction);
            HighlightAttackRangeTiles(currentOperator, direction);
            previousDirection = direction;
        }
    }

    private void HighlightInitialTiles(Vector2 defaultDirection)
    {
        HighlightAttackRangeTiles(currentOperator, defaultDirection);
        HighlightDirectionPhaseTiles(false);
    }

    private void DisplayGhost()
    {
        BuildManager.main.StartDragging();
    }

    private void SlowDownTime()
    {
        Time.timeScale = BuildManager.main.GetSlowTimeScale();
    }

    private void RotateOperatorSprite(Vector2 direction)
    {
        if (currentOperator == null || currentOperator.parentTransform == null)
        {
            Debug.LogError("Operator or its parent transform is missing for " + currentOperator?.OperatorName);
            return;
        }

        // Calculate rotation angle based on the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the parent transform, so the whole hierarchy rotates accordingly
        currentOperator.parentTransform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void StartDeployment(CharacterBio operatorBio, Plot plot)
    {
        currentPlot = plot;
        currentOperator = operatorBio;

        // Initial range highlight
        HighlightAttackRangeTiles(operatorBio, Vector2.left); // Default direction is left
        HighlightDirectionPhaseTiles(false);

        // Display ghost and make placement UI visible
        BuildManager.main.StartDragging();
        Time.timeScale = BuildManager.main.GetSlowTimeScale();
        StartCoroutine(WaitForDirection());
    }

    private void UpdateAttackRangePoint(CharacterBio operatorBio, Vector2 direction)
    {
        if (operatorBio.AttackRangePoint != null)
        {
            // Set the position of the attack range point to the center of the plot
            //operatorBio.AttackRangePoint.transform.position = currentPlot.GetPlotCenter();

            // Adjust the rotation to match the direction
            var attackRange = operatorBio.OperatorAttackRange;

            // Calculate rotation angle for the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            operatorBio.AttackRangePoint.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Scale the attack range point according to the attack range settings
            switch (attackRange.rangeShape)
            {
                case AttackRange.RangeShape.Circle:
                    operatorBio.AttackRangePoint.transform.localScale = new Vector3(attackRange.radius * 2, attackRange.radius * 2, 1);
                    break;
                case AttackRange.RangeShape.Rectangle:
                    operatorBio.AttackRangePoint.transform.localScale = new Vector3(attackRange.rectangleSize.x, attackRange.rectangleSize.y, 1);
                    break;
                case AttackRange.RangeShape.Line:
                    operatorBio.AttackRangePoint.transform.localScale = new Vector3(attackRange.lineLength, 0.1f, 1);
                    break;
                case AttackRange.RangeShape.Custom:
                    Debug.Log("Custom range shapes require custom visual setups.");
                    break;
            }

            operatorBio.AttackRangePoint.SetActive(true);
        }
    }

    public void HighlightAttackRangeTiles(CharacterBio operatorBio, Vector2 direction)
    {
        ClearHighlightedTiles(); // Clear previously highlighted tiles

        if (operatorBio.OperatorAttackRange != null)
        {
            // Ensure AttackRangePoint is properly updated
            UpdateAttackRangePoint(operatorBio, direction);

            // Use the updated position of AttackRangePoint as the center for range calculation
            Vector3 rangePointPosition = operatorBio.AttackRangePoint.transform.position;

            // Use `GetTargetsInRange` with AttackRangePoint as the center
            tilesInRange = operatorBio.OperatorAttackRange.GetTargetsInRange(
                rangePointPosition, tileLayerMask, direction).ToArray();

            Debug.Log("Tiles in range: " + tilesInRange.Length + " for direction: " + direction);

            foreach (var tile in tilesInRange)
            {
                Renderer tileRenderer = tile.GetComponent<Renderer>();

                if (tileRenderer != null)
                {
                    if (tileRenderer.material.HasProperty("_Color"))
                    {
                        originalColor = tileRenderer.material.color;

                        Color newColor = directionHighlightColor;
                        newColor.a = 1f; // Ensure full opacity
                        tileRenderer.material.color = newColor;

                        highlightedTiles.Add(tileRenderer);
                    }
                }
            }
        }
    }


    private void HighlightDirectionPhaseTiles(bool highlight)
    {
        if (tilesInRange == null || tilesInRange.Length == 0)
        {
            Debug.LogWarning("No tiles in range to highlight.");
            return;
        }

        foreach (var tile in tilesInRange)
        {
            Renderer tileRenderer = tile.GetComponent<Renderer>();
            if (tileRenderer != null)
            {
                tileRenderer.material.color = highlight ? directionHighlightColor : originalColor;
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        foreach (Renderer tileRenderer in highlightedTiles)
        {
            if (tileRenderer != null)
            {
                tileRenderer.material.color = originalColor;
            }
        }
        highlightedTiles.Clear();
    }

    private IEnumerator WaitForDirection()
    {
        isChoosingDirection = true;
        directionUI.SetActive(true);

        HighlightDirectionPhaseTiles(true);

        while (isChoosingDirection)
        {
            yield return null;
        }

        FinalizeDeployment();
    }

    public void ChooseDirection(Vector2 direction)
    {
        if (currentOperator != null)
        {
            currentOperator.SetDirection(direction);
            UpdateAttackRangePoint(currentOperator, direction);
            HighlightAttackRangeTiles(currentOperator, direction);
        }

        directionUI.SetActive(false);
        HighlightDirectionPhaseTiles(false);
        isChoosingDirection = false;
    }

    public void FinalizeDeployment()
    {
        BuildManager.main.StopDragging();

        HighlightAttackRangeTiles(currentOperator, currentOperator.GetDirection());

        ClearHighlightedTiles();

        currentOperator.DisplayOperatorInfo();
        EventManager.CharacterSelected(currentOperator);

        Time.timeScale = BuildManager.main.GetDefaultTimeScale();

        EventManager.DeploymentCompleted();
    }

    public bool IsChoosingDirection()
    {
        return isChoosingDirection;
    }
}
