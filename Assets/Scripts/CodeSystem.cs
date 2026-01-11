using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; // Necesar pentru IEnumerator

public class CodeSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject errorPopup; // Trage aici obiectul ErrorPopup
    public string codCorect = "1234";
    public string numeScenaFinala = "WinScene";
    public MonoBehaviour playerMovementScript;

    public void VerificaCodul()
    {
        if (inputField.text == codCorect)
        {
            // Teleportare imediată
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
        }
        else
        {
            StartCoroutine(ArataMesajEroare());
        }
    }
    public void InchidePanel()
    {
        // Dezactivăm Canvas-ul (acest obiect)
        this.gameObject.SetActive(false);

        // Reactivăm mișcarea jucătorului
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Blocăm din nou cursorul pentru a putea juca
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Opțional: Curățăm textul din InputField la ieșire
        inputField.text = "";
    }
    IEnumerator ArataMesajEroare()
    {
        errorPopup.SetActive(true);
        inputField.text = ""; // Curăță ce a scris jucătorul
        yield return new WaitForSeconds(2f); // Mesajul stă 2 secunde
        errorPopup.SetActive(false);
    }
}