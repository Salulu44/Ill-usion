using UnityEngine;
using UnityEngine.UI;

public class DoodleWeaponUIScript : MonoBehaviour
{
    [SerializeField] DoodleWeaponScript doodleWeaponScript;
    Image doodleWeaponUIRenderer;
    void Start()
    {
        doodleWeaponScript.OnShoot += CheckProjectileAmount; 
        DoodleWeaponScript.OnDestoy += CheckProjectileAmount;
        doodleWeaponUIRenderer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckProjectileAmount() 
    {
        Debug.Log("projectiles " + DoodleWeaponScript.projectiles.Count);
        doodleWeaponUIRenderer.fillAmount = 1 -((float)DoodleWeaponScript.projectiles.Count / doodleWeaponScript.projectileMaxAmount) ;
    }
}
