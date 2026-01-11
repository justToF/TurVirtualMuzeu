using UnityEngine;
using System.Collections;

public class CharacterWinLogic : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Această funcție va fi apelată când se încarcă scena de victorie
    void Start()
    {
        StartCoroutine(WaitAndDance());
    }

    IEnumerator WaitAndDance()
    {
        // Așteptăm 5 secunde
        yield return new WaitForSeconds(5f);

        // Activăm trigger-ul pentru dans
        if (animator != null)
        {
            animator.SetTrigger("startDancing");
        }
    }
}