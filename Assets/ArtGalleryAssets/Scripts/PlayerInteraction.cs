using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Setari Interactiune")]
    public float interactionDistance = 5f;

    [Header("Referinte UI")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoText;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            ItemData data = hit.collider.GetComponent<ItemData>();

            if (data != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    infoText.text = data.description;
                    infoPanel.SetActive(true);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                infoPanel.SetActive(false);
            }
        }
    }
}