using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoot : MonoBehaviour
{
    public GameObject playerLook;
    public AudioSource AmmoPick;
    public GunShoot m_gunsh;
    public int healthDifference;
    public int ammoDifference;
    public Transform enemyLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerLook.transform.position, playerLook.transform.forward, out hit, 100f) && Input.GetKeyDown(KeyCode.E) && enemyLeft.childCount == 0)
        {
            if(hit.transform.tag == "AmmoBox")
            {
                if(m_gunsh.ammo < m_gunsh.thisAmmo)
                {
                    ammoDifference = m_gunsh.thisAmmo - m_gunsh.ammo;
                    m_gunsh.ammo += ammoDifference;
                    AmmoPick.Play();
                }
            }

        }
        
    }
}
