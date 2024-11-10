using System.Collections;
using UnityEngine;

public class CharacterClickable : MonoBehaviour
{
    private CharacterBio characterBio;
    private DeployHandler deployHandler;

    private void Awake()
    {
        // Attempt to find the CharacterBio component attached to the same GameObject
        characterBio = GetComponent<CharacterBio>();
        deployHandler = FindObjectOfType<DeployHandler>(); // Finds the DeployHandler in the scene

        if (characterBio == null)
        {
            Debug.LogError("CharacterBio component not found on " + gameObject.name);
        }

        if (characterBio == null)
        {
            Debug.LogError("CharacterBio component not found on " + gameObject.name);
        }
        else
        {
            Debug.Log("CharacterBio component found on " + gameObject.name);
        }
    }

    private void Update()
    {
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Convert screen point to world point for accurate raycasting
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Perform a 2D raycast at the mouse position (use Physics.Raycast for 3D)
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // Adjust to Physics.Raycast for 3D

            // Check if the raycast hits a collider
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Tower clicked: " + gameObject.name);

                // Call SelectCharacter if the characterBio component is present
                if (characterBio != null)
                {
                    characterBio.SelectCharacter();
                    Vector2 currentDirection = characterBio.GetDirection();
                    deployHandler.HighlightAttackRangeTiles(characterBio, currentDirection);
                }
                else
                {
                    Debug.LogError("CharacterBio component is missing on " + gameObject.name);
                }
            }
        }
    }
}
