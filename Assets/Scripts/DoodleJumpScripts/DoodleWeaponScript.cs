using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
public class DoodleWeaponScript : MonoBehaviour
{
    [SerializeField] private DoodleProjectileScript projectileScript;
    [SerializeField] public int projectileMaxAmount;
    [SerializeField] float shootKnockback;
    public event Action OnShoot;
    public static Action OnDestoy;
    public static List<DoodleProjectileScript> projectiles = new List<DoodleProjectileScript>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && projectiles.Count < projectileMaxAmount) 
        {

          DoodleProjectileScript doodleProjectileScript = Instantiate(projectileScript,transform.position,Quaternion.identity);
          Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
          doodleProjectileScript.SetDirection(direction);
          //  Debug.Log("Shoot KnockbackDirection " + (-direction));
          GetComponent<Rigidbody2D>().AddForce(-direction * shootKnockback, ForceMode2D.Impulse);
          projectiles.Add(doodleProjectileScript);
          OnShoot?.Invoke();
        }
        Debug.Log("Count " + projectiles.Count);
    }
}
