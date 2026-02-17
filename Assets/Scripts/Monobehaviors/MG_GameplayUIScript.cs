using UnityEngine;

public class MG_GameplayUIScript : MonoBehaviour
{
    [SerializeField] Animator bloodAnimator;
    [SerializeField] HealthScript playerHealthScript;
    void Start()
    {
        playerHealthScript.OnDamaged += ShowBloodAnimation;
    }
    void ShowBloodAnimation()
    {
        bloodAnimator.SetTrigger("BloodAnimation");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
