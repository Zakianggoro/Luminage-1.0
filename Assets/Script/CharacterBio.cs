using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBio : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] private string operatorName;
    [SerializeField] private string operatorClass;
    [SerializeField] private string operatorRange;
    [SerializeField] private int level = 1;
    [SerializeField] private int atk;
    [SerializeField] private int def;
    [SerializeField] private int blockCount;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float currentEnergy = 0;
    [SerializeField] private SpriteRenderer operatorSpriteRenderer;

    [Header("Character Image")]
    [SerializeField] private Image operatorImage;
    [SerializeField] private Image operatorClassImage;
    [SerializeField] private Image operatorRangeImage;

    [Header("Character Unique")]
    [SerializeField] private Skill operatorSkill;
    [SerializeField] private Trait operatorTrait;
    [SerializeField] private Talent operatorTalent;
    [SerializeField] private AttackRange operatorAttackRange;
    [SerializeField] private GameObject attackRangePoint;
    public Transform parentTransform;

    [Header("Character Skills")]
    [SerializeField] private List<SkillBase> operatorSkills;

    private List<Renderer> highlightedTiles = new List<Renderer>(); // Store highlighted tiles

    private SkillBase operatorSkillBase;
    private int maxBlockCount;

    private void Awake()
    {
        operatorSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        maxBlockCount = blockCount;

        if (operatorSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not assigned in " + operatorName);
        }

        // Activate first skill
        if (operatorSkills.Count > 0)
        {
            //operatorSkills[0].ActivateSkill(this);  // Activates the first skill in the list
            Debug.Log("Skill 1 activate on awake");
        }

        if (operatorAttackRange == null)
        {
            Debug.LogError("OperatorAttackRange is not assigned in " + operatorName);
        }

        if (attackRangePoint == null)
        {
            Debug.LogError("Attack range GameObject is not assigned in " + operatorName);
        }
    }


    // You can now attach multiple skills to the character dynamically or via Inspector
    public void ActivateSkill(int skillIndex)
    {
        if (skillIndex < operatorSkills.Count && operatorSkills[skillIndex] != null)
        {
            operatorSkills[skillIndex].ActivateSkill(this);
        }
    }

    public void DeactivateSkill(int skillIndex)
    {
        if (skillIndex < operatorSkills.Count && operatorSkills[skillIndex] != null)
        {
            operatorSkills[skillIndex].DeactivateSkill(this);
        }
    }

    public GameObject AttackRangePoint
    {
        get { return attackRangePoint; }
        set { attackRangePoint = value; }
    }

    public SpriteRenderer OperatorSpriteRenderer
    {
        get { return operatorSpriteRenderer; }
        set { operatorSpriteRenderer = value; }
    }

    public AttackRange OperatorAttackRange
    {
        get { return operatorAttackRange;  }
        set { operatorAttackRange = value; }
    }

    // Getter and Setter for OperatorName
    public string OperatorName
    {
        get { return operatorName; }
        set { operatorName = value; }
    }

    // Getter and Setter for OperatorClass
    public string OperatorClass
    {
        get { return operatorClass; }
        set { operatorClass = value; }
    }

    public string OperatorRange
    {
        get { return operatorRange; }
        set { operatorRange = value; }
    }

    // Getter and Setter for Level
    public int Level
    {
        get { return level; }
        set { level = Mathf.Max(1, value); } // Ensure level is at least 1
    }

    // Getter and Setter for MaxHealth
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = Mathf.Max(1, value); } // Ensure max health is at least 1
    }

    // Getter and Setter for CurrentHealth
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, maxHealth); } // Limit current health between 0 and maxHealth
    }

    // Getter and Setter for ATK
    public int ATK
    {
        get { return atk; }
        set { atk = Mathf.Max(0, value); }
    }

    // Getter and Setter for DEF
    public int DEF
    {
        get { return def; }
        set { def = Mathf.Max(0, value); }
    }

    // Getter and Setter for BlockCount
    public int BlockCount
    {
        get { return blockCount; }
        set { blockCount = Mathf.Max(0, value); }
    }

    public int MaxBlockCount
    {
        get { return maxBlockCount; }
        set { maxBlockCount = Mathf.Max(0, value); }
    }

    public Image OperatorImage
    {
        get { return operatorImage; }
        set { operatorImage = value; }
    }

    public Image OperatorClassImage
    {
        get { return operatorClassImage; }
        set { operatorClassImage = value; }
    }

    public Image OperatorRangeImage
    {
        get { return operatorRangeImage; }
        set { operatorRangeImage = value; }
    }

    public Skill OperatorSkill
    {
        get { return operatorSkill; }
        set { operatorSkill = value; }
    }

    public Trait OperatorTrait
    {
        get { return operatorTrait; }
        set { operatorTrait = value; }
    }

    public Talent OperatorTalent
    {
        get { return operatorTalent; }
        set { operatorTalent = value; }
    }

    public SkillBase OperatorSkillBase
    {
        get { return operatorSkillBase; }
        set { operatorSkillBase = value; }
    }

    public List<SkillBase> OperatorSkills
    {
        get { return operatorSkills; }
    }

    public void SelectCharacter()
    {
        Debug.Log("Character selected: " + OperatorName);
        EventManager.CharacterSelected(this);
    }

    public void HandleEnergyAccumulation()
    {
        switch (OperatorSkillBase.energyType)
        {
            case SkillBase.EnergyType.Automatic:
                operatorSkillBase.AccumulateEnergy(Time.deltaTime);
                break;
            case SkillBase.EnergyType.OnAttack:
                // Add logic for energy on attack
                break;
            case SkillBase.EnergyType.OnHit:
                // Add logic for energy on hit
                break;
        }
    }

    // Visualize the attack range in the scene view
    void OnDrawGizmosSelected()
    {
        if (operatorAttackRange != null && attackRangePoint != null)
        {
            Vector2 direction = GetDirection(); // Convert rotation to 2D direction
            operatorAttackRange.DrawRangeGizmo(attackRangePoint.transform.position, direction);
        }
    }

    // Get the list of targets within the operator's attack range
    public List<Collider2D> GetTargetsInRange()
    {
        if (operatorAttackRange != null)
        {
            Vector2 direction = GetDirection(); // Convert rotation to 2D direction
            return operatorAttackRange.GetTargetsInRange(attackRangePoint.transform.position, LayerMask.GetMask("Enemy"), direction);
        }

        return new List<Collider2D>();
    }

    public void ActivateSkill()
    {
        if (operatorSkill != null && operatorSkillBase.IsSkillReady())
        {
            operatorSkillBase.ActivateSkill(this);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        // Example of rotating an operator in 2D
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log($"{operatorName} is now facing {direction}");
    }

    public Vector2 GetDirection()
    {
        // Assumes the operator faces based on their rotation, using 2D angles
        return new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
    }

    public void FlipX()
    {

    }

    public void DisplayOperatorInfo()
    {
        SelectCharacter();
    }

    public void HighlightAttackRangeTiles()
    {
        ClearHighlightedTiles(); // Clear previous highlights

        if (operatorAttackRange != null && attackRangePoint != null)
        {
            Vector2 direction = GetDirection(); // Get operator's facing direction
            Vector3 rangePointPosition = attackRangePoint.transform.position;

            // Get tiles in attack range
            Collider2D[] tilesInRange = operatorAttackRange.GetTargetsInRange(rangePointPosition, LayerMask.GetMask("Tile"), direction).ToArray();

            foreach (var tile in tilesInRange)
            {
                Renderer tileRenderer = tile.GetComponent<Renderer>();
                if (tileRenderer != null)
                {
                    Color highlightColor = Color.red; // Set your highlight color
                    tileRenderer.material.color = highlightColor;
                    highlightedTiles.Add(tileRenderer);
                }
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        foreach (Renderer tileRenderer in highlightedTiles)
        {
            if (tileRenderer != null)
            {
                tileRenderer.material.color = Color.white; // Reset to default color
            }
        }
        highlightedTiles.Clear();
    }
}
