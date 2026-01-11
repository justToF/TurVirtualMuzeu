using UnityEngine;
using System.Collections;

public class WinSceneManager : MonoBehaviour
{
    public Animator characterAnimator; // Trage caracterul aici în Inspector
    public ParticleSystem fireworks;   // Trage artificiile aici

    void Start()
    {
        // Când se încarcă scena, pornește numărătoarea
        StartCoroutine(DelayedCelebration());
    }

    IEnumerator DelayedCelebration()
    {
        // Așteaptă 5 secunde în timp ce jucătorul se uită la muzeu/caracter
        yield return new WaitForSeconds(5f);

        // Pornește dansul
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("startDancing");
        }

        // Pornește artificiile
        if (fireworks != null)
        {
            fireworks.Play();
        }

        Debug.Log("Sărbătoarea a început!");
    }
}