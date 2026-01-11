using UnityEngine;
// Adaugă asta dacă folosești noul Starter Assets de la Unity
// using Unity.StarterAssets; 

public class TerminalInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public GameObject uiCanvas;

    // Trage aici obiectul FirstPerson sau scriptul de mișcare direct
    public MonoBehaviour playerMovementScript;

    private bool isMenuOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
            {
                if (hit.collider.CompareTag("Terminal"))
                {
                    ToggleMenu();
                }
            }
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        uiCanvas.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            // Dezactivăm mișcarea și rotația
            playerMovementScript.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Reactivăm mișcarea când închidem meniul
            playerMovementScript.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}