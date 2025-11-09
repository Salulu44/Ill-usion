using UnityEngine;

public class DoodleWeaponScript : MonoBehaviour
{
    [SerializeField] private DoodleProjectileScript projectileScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
          DoodleProjectileScript doodleProjectileScript = Instantiate(projectileScript,transform.position,Quaternion.identity);
          doodleProjectileScript.SetDirection((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);
        }
        
    }
}
