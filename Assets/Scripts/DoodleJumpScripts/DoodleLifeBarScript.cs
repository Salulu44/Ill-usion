using UnityEngine;
using UnityEngine.UI;

public class DoodleLifeBarScript : MonoBehaviour
{
    [SerializeField] HealthScript healthScript;
    private Image lifeBar;
    void Start()
    {
        healthScript.OnDamaged += FillAmount;
        lifeBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FillAmount()
    {
        float fillAmount = healthScript.currentHealth / healthScript.maxHealth;
        lifeBar.fillAmount = fillAmount;
    }
    public void SetFillAmount(float fillAmount) 
    {
        lifeBar.fillAmount = fillAmount;
    }
}
