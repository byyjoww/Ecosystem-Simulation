using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSAttackController : MonoBehaviour
{
    [SerializeField] private BoolScriptableEvent OnAim;
    [SerializeField] private BoolScriptableEvent OnShoot;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float firerate;
    [SerializeField, Range(0,2)] private float shotDelay;

    private float cooldown = 0f;
    public bool startedAiming;
    public bool isAiming;
    private bool isShooting;

    private void Start()
    {
        isAiming = false;
        OnShoot.OnRaise += ToggleShooting;
        OnAim.OnRaise += StartAim;
    }

    private void ToggleShooting(bool shoot)
    {
        isShooting = shoot;        
    }

    private void Shoot()
    {
        if (!isAiming)
        {
            return;
        }

        if (cooldown > 0)
        {
            return;
        }

        Instantiate(projectile, firepoint.position, firepoint.rotation);
        cooldown = firerate;
    }

    private void StartAim(bool aim)
    {
        if (aim)
        {
            startedAiming = true;
            StartCoroutine(Aim());
        }
        else
        {
            startedAiming = aim;
            isAiming = aim;
        }            
    }

    IEnumerator Aim()
    {
        yield return new WaitForSeconds(shotDelay);
        isAiming = startedAiming;
        yield return null;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            cooldown = 0;
        }

        if (isShooting)
        {
            Shoot();
        }        
    }
}
