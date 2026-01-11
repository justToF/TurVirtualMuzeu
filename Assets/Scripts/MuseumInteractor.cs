using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MuseumInteractor : MonoBehaviour
{
    [Header("Raycast")]
    public Camera playerCamera;
    public float interactDistance = 3f;
    public LayerMask interactLayer = ~0;

    [Header("UI - Panel info")]
    public GameObject infoPanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("UI - Hint (hover)")]
    public TMP_Text interactHintText;
    public string hintMessage = "Apasă E pentru detalii";

    [Header("Crosshair")]
    public Image crosshairImage;
    public Vector2 crosshairNormalSize = new Vector2(6, 6);
    public Vector2 crosshairHoverSize = new Vector2(12, 12);
    public float crosshairLerpSpeed = 12f;
    public Color crosshairNormalColor = Color.white;
    public Color crosshairHoverColor = Color.green;

    [Header("Highlight")]
    [Range(1f, 2f)]
    public float tintMultiplier = 1.2f;

    [Header("Freeze player input while panel open")]
    public PlayerMovement playerMovement; // tragi Player-ul aici

    private ExhibitInfo currentHover;
    private bool panelOpen;

    private readonly List<Material> highlightedMats = new List<Material>();
    private readonly Dictionary<Material, Color> originalColors = new Dictionary<Material, Color>();

    void Start()
    {
        if (playerCamera == null) playerCamera = Camera.main;

        // panel off la start
        if (infoPanel != null) infoPanel.SetActive(false);
        SetHint(false);

        // asigură-te că input-ul e ON la start
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null) playerMovement.inputEnabled = true;

        if (crosshairImage != null)
        {
            crosshairImage.rectTransform.sizeDelta = crosshairNormalSize;
            crosshairImage.color = crosshairNormalColor;
        }
    }

    void Update()
    {
        UpdateCrosshair();

        if (panelOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                HidePanel();
            return;
        }

        HoverCheck();

        if (currentHover != null && Input.GetKeyDown(KeyCode.E))
            ShowPanel(currentHover);
    }

    void HoverCheck()
    {
        ExhibitInfo hitInfo = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
            hitInfo = hit.collider.GetComponentInParent<ExhibitInfo>();

        if (hitInfo != currentHover)
        {
            ClearHighlight();
            currentHover = hitInfo;

            if (currentHover != null)
            {
                ApplyHighlight(currentHover);
                SetHint(true);
            }
            else
            {
                SetHint(false);
            }
        }
    }

    void UpdateCrosshair()
    {
        if (crosshairImage == null) return;

        bool hovering = (currentHover != null && !panelOpen);

        Vector2 targetSize = hovering ? crosshairHoverSize : crosshairNormalSize;
        Color targetColor = hovering ? crosshairHoverColor : crosshairNormalColor;

        crosshairImage.rectTransform.sizeDelta =
            Vector2.Lerp(crosshairImage.rectTransform.sizeDelta, targetSize, Time.deltaTime * crosshairLerpSpeed);

        crosshairImage.color =
            Color.Lerp(crosshairImage.color, targetColor, Time.deltaTime * crosshairLerpSpeed);
    }

    void ApplyHighlight(ExhibitInfo info)
    {
        Renderer[] rends = (info.renderersToHighlight != null && info.renderersToHighlight.Length > 0)
            ? info.renderersToHighlight
            : info.GetComponentsInChildren<Renderer>();

        foreach (var r in rends)
        {
            if (r == null) continue;

            foreach (var mat in r.materials)
            {
                if (mat == null) continue;

                bool hasBase = mat.HasProperty("_BaseColor");
                bool hasColor = mat.HasProperty("_Color");
                if (!hasBase && !hasColor) continue;

                if (!originalColors.ContainsKey(mat))
                {
                    Color original = hasBase ? mat.GetColor("_BaseColor") : mat.GetColor("_Color");
                    originalColors[mat] = original;
                }

                Color baseCol = originalColors[mat];
                Color newCol = new Color(
                    Mathf.Clamp01(baseCol.r * tintMultiplier),
                    Mathf.Clamp01(baseCol.g * tintMultiplier),
                    Mathf.Clamp01(baseCol.b * tintMultiplier),
                    baseCol.a
                );

                if (hasBase) mat.SetColor("_BaseColor", newCol);
                else mat.SetColor("_Color", newCol);

                highlightedMats.Add(mat);
            }
        }
    }

    void ClearHighlight()
    {
        foreach (var mat in highlightedMats)
        {
            if (mat == null) continue;
            if (!originalColors.TryGetValue(mat, out var original)) continue;

            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", original);
            else if (mat.HasProperty("_Color")) mat.SetColor("_Color", original);
        }

        highlightedMats.Clear();
        originalColors.Clear();
    }

    void SetHint(bool show)
    {
        if (interactHintText == null) return;

        interactHintText.gameObject.SetActive(show);
        if (show) interactHintText.text = hintMessage;
    }

    public void ShowPanel(ExhibitInfo info)
    {
        panelOpen = true;

        if (infoPanel != null) infoPanel.SetActive(true);
        if (titleText != null) titleText.text = info.title;
        if (bodyText != null) bodyText.text = info.description;

        SetHint(false);

        // BLOCARE INPUT (asta oprește camera 100%)
        if (playerMovement != null) playerMovement.inputEnabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePanel()
    {
        panelOpen = false;

        if (infoPanel != null) infoPanel.SetActive(false);

        // DEBLOCARE INPUT
        if (playerMovement != null) playerMovement.inputEnabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
