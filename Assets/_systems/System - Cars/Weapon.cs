using System.Timers;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Scriptable Object/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Details")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float cooldownSeconds;

    private const float initialGlobalCooldown = 5f;
    public Transform firepoint { get; set; }    

    private bool isOnCooldown;
    public bool IsOnCooldown => isOnCooldown;

    public void Shoot()
    {
        GameObject obj = GameObject.Instantiate(projectile, firepoint.position, firepoint.rotation);
        isOnCooldown = true;
        Debug.Log("Weapon has fired.");
        PlaceOnCooldown(cooldownSeconds);        
    }    

    private void PlaceOnCooldown(float cooldownTime = initialGlobalCooldown)
    {        
        Timer timer = new Timer();
        timer.Interval = cooldownTime * 1000;
        timer.Enabled = true;
        timer.Elapsed += Recover;
        Debug.Log("Cooldown Timer Started.");
    }

    public void Recover(object o, ElapsedEventArgs e)
    {
        Debug.Log("Weapon has recovered.");
        isOnCooldown = false;
    }
}
