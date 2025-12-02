using UnityEngine;
using UnityEngine.UI;

public class DoodleLifeBarScript : MonoBehaviour
{
    [SerializeField] HealthScript healthScript;
    private Image lifeBar;
    [SerializeField,Range(0f,1f)] float lifeBarChangeChance;
    [SerializeField] LifebarEnemyScript lifebarEnemyScript;
    Color[] colors = { Color.aliceBlue, Color.beige, Color.blanchedAlmond , Color.red};
    Image image;
    void Start()
    {
        healthScript.OnDamaged += FillAmount;
        lifeBar = GetComponent<Image>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FillAmount()
    {
        float fillAmount = healthScript.currentHealth / healthScript.maxHealth;
        lifeBar.fillAmount = fillAmount;
        if (GameManagerScript.Instance.HasLuck(lifeBarChangeChance)) 
        {
            //Lifebar fights against you
            image.color = colors[Random.Range(0, colors.Length)];
            lifebarEnemyScript.gameObject.SetActive(true);
        }
    }
    public void SetFillAmount(float fillAmount) 
    {
        lifeBar.fillAmount = fillAmount;
    }
}
