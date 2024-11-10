using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public enum PlotType
    {
        Ground,
        Range,
        Both // For plots that can accommodate both ground and ranged operators
    }

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color canDeployColor;
    [SerializeField] private Color rangeColor;
    [SerializeField] private bool isDeployable = true;

    [Header("Plot Type")]
    [SerializeField] private PlotType plotType;  // Specify plot type here in inspector

    private GameObject tower;
    private Color startColor;
    private bool canPlace = false;
    private GameObject currentRangePoint;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        if (!isDeployable)
        {
            canPlace = false;
        }
        else
        {
            BuildManager.main.SetCurrentPlot(this);
            canPlace = true;
        }
    }

    private void OnMouseExit()
    {
        if (BuildManager.main.IsDragging())
        {
            BuildManager.main.ClearCurrentPlot();
        }
        canPlace = false;
        HideAttackRange();
    }

    // Checks if a tower can be placed on this plot
    public bool CanPlaceTower(Tower tower)
    {
        if (!canPlace || this.tower != null || !isDeployable) return false;

        // Ensure the plot type is compatible with the tower's deploy type
        return plotType == PlotType.Both || (plotType == PlotType.Ground && tower.onGround) || (plotType == PlotType.Range && tower.onRange);
    }

    public GameObject PlaceTower(GameObject towerPrefab, AttackRange attackRange)
    {
        if (tower == null && isDeployable)
        {
            Vector3 plotCenter = sr.bounds.center;
            tower = Instantiate(towerPrefab, plotCenter, Quaternion.identity);

            SetupAttackRange(attackRange);
            return tower;
        }
        return null;
    }

    private void SetupAttackRange(AttackRange attackRange)
    {
        currentRangePoint = tower.GetComponentInChildren<CharacterBio>()?.AttackRangePoint;
        if (currentRangePoint != null)
        {
            switch (attackRange.rangeShape)
            {
                case AttackRange.RangeShape.Circle:
                    currentRangePoint.transform.localScale = new Vector3(attackRange.radius * 2, attackRange.radius * 2, 1);
                    break;
                case AttackRange.RangeShape.Rectangle:
                    currentRangePoint.transform.localScale = new Vector3(attackRange.rectangleSize.x, attackRange.rectangleSize.y, 1);
                    break;
                case AttackRange.RangeShape.Line:
                    currentRangePoint.transform.localScale = new Vector3(attackRange.lineLength, 0.1f, 1);
                    break;
                case AttackRange.RangeShape.Custom:
                    Debug.Log("Custom range shapes require custom visual setups.");
                    break;
            }

            currentRangePoint.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Attack range point not found on operator.");
        }
    }

    public void HighlightPlot(PlotType requiredType)
    {
        // Highlight the plot based on the required type
        if ((plotType == requiredType || plotType == PlotType.Both) && isDeployable)
        {
            sr.color = canDeployColor;
        }
    }

    public void ResetColor()
    {
        sr.color = startColor;
    }

    private void ShowAttackRange()
    {
        if (currentRangePoint != null)
        {
            currentRangePoint.SetActive(true);
        }
    }

    private void HideAttackRange()
    {
        if (currentRangePoint != null)
        {
            currentRangePoint.SetActive(false);
        }
    }

    public Vector3 GetPlotCenter()
    {
        return sr.bounds.center;
    }
}
