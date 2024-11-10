using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Plot;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;  // Array of available towers
    [SerializeField] private GameObject operatorGhostPrefab; // Ghost of operator sprite during drag
    [SerializeField] private Button[] towerButtons; // Buttons to trigger dragging

    [Header("Deployment Settings")]
    public int maxDeployments = 8;  // Maximum allowed deployments
    private int currentDeployments = 0;  // Track current deployments
    private Dictionary<Tower, bool> deployedOperators = new Dictionary<Tower, bool>(); // Track each operator's deployment status

    private GameObject currentGhost;
    private Plot currentPlot;  // Reference to the plot we're currently hovering over
    private int selectedTower = 0;
    private bool isDragging = false;

    private float defaultTimeScale = 1.0f;  // Store the default timescale value
    private float slowTimeScale = 0.3f;     // Define a slower timescale for dragging

    private void Awake()
    {
        main = this;

        if (towers.Length == 0)
        {
            Debug.LogError("No towers assigned in the BuildManager!");
        }

        foreach (Tower tower in towers)
        {
            if (tower.prefab == null)
            {
                Debug.LogError("Tower prefab is missing for one of the towers!");
            }
            deployedOperators[tower] = false;  // Initialize deployment status as false
        }

        foreach (Button btn in towerButtons)
        {
            AddHoldListener(btn);
        }
    }

    private void Update()
    {
        UpdateButtonStates(); // Update button states based on deployment and currency

        if (isDragging)
        {
            if (currentGhost != null)
            {
                if (currentPlot != null)
                {
                    Vector3 plotCenter = currentPlot.GetPlotCenter(); // Get the center of the plot
                    currentGhost.transform.position = plotCenter;
                }
                else
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    currentGhost.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                }
            }
        }
    }

    // Add event listeners to detect when the button is pressed and held
    private void AddHoldListener(Button btn)
    {
        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();

        // Add listener for pointer down (hold start)
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((eventData) => OnHoldStart(btn));
        trigger.triggers.Add(entryPointerDown);

        // Add listener for pointer up (release hold)
        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((eventData) => OnHoldEnd());
        trigger.triggers.Add(entryPointerUp);
    }

    // Called when the button is clicked and held
    private void OnHoldStart(Button btn)
    {
        int towerIndex = System.Array.IndexOf(towerButtons, btn);
        SetSelectedTower(towerIndex);
        StartDragging();
    }

    // Called when the button is released
    private void OnHoldEnd()
    {
        if (currentPlot != null)
        {
            TryPlaceTower();
        }
        else
        {
            Debug.Log("No valid plot under mouse on hold end. Cancelling placement.");
        }

        StopDragging();
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void StartDragging()
    {
        isDragging = true;
        Time.timeScale = slowTimeScale;

        if (operatorGhostPrefab != null && currentGhost == null)
        {
            currentGhost = Instantiate(operatorGhostPrefab);
        }

        Tower selectedTower = GetSelectedTower();
        HighlightValidPlots(selectedTower.onGround, selectedTower.onRange);
        // Highlight only valid plots for the operator
    }

    public void StopDragging()
    {
        isDragging = false;

        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }

        ResetPlotHighlights();  // Reset plot colors once placement is finished
    }

    private void TryPlaceTower()
    {
        if (currentPlot != null && currentPlot.CanPlaceTower(GetSelectedTower()) && CanDeploy(selectedTower))
        {
            PlaceTower();
        }
        else
        {
            Debug.Log("Cannot place tower here or deployment limit reached.");
        }
    }

    private bool CanDeploy(int towerIndex)
    {
        Tower tower = towers[towerIndex];
        if (currentDeployments < maxDeployments && !deployedOperators[tower])
        {
            return true;
        }
        return false;
    }

    public void SetCurrentPlot(Plot plot)
    {
        currentPlot = plot;

        if (currentGhost != null)
        {
            currentGhost.transform.position = plot.transform.position;
        }
    }

    public void ClearCurrentPlot()
    {
        currentPlot = null;
    }

    private void PlaceTower()
    {
        Tower towerToBuild = GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Not enough currency to place tower.");
            Time.timeScale = defaultTimeScale;
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        if (currentPlot == null)
        {
            Debug.LogError("currentPlot is null! Cannot place tower.");
            return;
        }

        GameObject placedTower = currentPlot.PlaceTower(towerToBuild.prefab, towerToBuild.prefab.GetComponentInChildren<CharacterBio>().OperatorAttackRange);

        if (placedTower != null)
        {
            // Set reference to the instantiated operator
            towerToBuild.SetPlacedOperator(placedTower);

            CharacterBio bio = placedTower.GetComponentInChildren<CharacterBio>();
            if (bio != null)
            {
                bio.DisplayOperatorInfo();
                EventManager.CharacterSelected(bio);

                DeployHandler deployHandler = FindObjectOfType<DeployHandler>();
                deployHandler.StartDeployment(bio, currentPlot);
            }

            deployedOperators[towerToBuild] = true;
            currentDeployments++;
            towerButtons[selectedTower].gameObject.SetActive(false);
        }

        OperatorHealth operatorHealth = placedTower.GetComponentInChildren<OperatorHealth>();
        if (operatorHealth != null)
        {
            operatorHealth.SetAssociatedTower(towerToBuild);
        }

        DeployDirection deploy = FindObjectOfType<DeployDirection>();
        if (deploy != null)
        {
            deploy.SetAssociatedTower(towerToBuild);
        }
    }


    // Call this when an operator is removed (if your game supports recalling operators)
    public void RecallOperator(Tower tower)
    {
        if (deployedOperators[tower])
        {
            GameObject placedOperator = tower.GetPlacedOperator();
            if (placedOperator != null)
            {
                Destroy(placedOperator);  // Destroy the operator GameObject
                tower.ClearPlacedOperator();  // Clear the reference in Tower
            }

            deployedOperators[tower] = false;
            currentDeployments--;

            int index = System.Array.IndexOf(towers, tower);
            if (index >= 0 && index < towerButtons.Length)
            {
                towerButtons[index].gameObject.SetActive(true); // Show the button again
            }
        }
    }


    private void UpdateButtonStates()
    {
        for (int i = 0; i < towers.Length; i++)
        {
            Button btn = towerButtons[i];
            Tower tower = towers[i];

            if (btn == null) continue;

            // Dim the button if currency is insufficient
            Image buttonImage = btn.GetComponent<Image>();
            if (LevelManager.main.currency < tower.cost)
            {
                buttonImage.color = new Color(1, 1, 1, 0.5f); // Dim (reduce alpha)
                btn.interactable = false;
            }
            else
            {
                buttonImage.color = Color.white; // Normal color
                btn.interactable = true;
            }
        }
    }

    private void HighlightValidPlots(bool onGround, bool onRange)
    {
        foreach (Plot plot in FindObjectsOfType<Plot>())
        {
            if (onGround)
            {
                plot.HighlightPlot(Plot.PlotType.Ground);
            }
            if (onRange)
            {
                plot.HighlightPlot(Plot.PlotType.Range);
            }
        }
    }


    private void ResetPlotHighlights()
    {
        foreach (Plot plot in FindObjectsOfType<Plot>())
        {
            plot.ResetColor();
        }
    }


    public bool IsDragging()
    {
        return isDragging;
    }

    public float GetDefaultTimeScale()
    {
        return defaultTimeScale;
    }

    public float GetSlowTimeScale()
    {
        return slowTimeScale;
    }
}
