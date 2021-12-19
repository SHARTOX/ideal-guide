using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLoot : MonoBehaviour
{
    public GameObject playerLook;
    public CharacterMovement m_char;
    public AudioSource HealthPick;
    public AudioSource armorPick;
    public int healthDifference;
    public int armorDifference;
    public int ammoDifference;
    public Transform enemyLeft;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerLook.transform.position, playerLook.transform.forward, out hit, 100f) && Input.GetKeyDown(KeyCode.E) && enemyLeft.childCount == 0)
        {
            if(hit.transform.tag == "HealthBox")
            {
                if(m_char.health < 100)
                {
                    healthDifference = 100 - m_char.health;

                    m_char.health += healthDifference;
                    HealthPick.Play();
                }

                if(m_char.armor < 100)
                {
                    armorDifference = 100 - m_char.armor;
                    m_char.armor += armorDifference;

                    armorPick.Play();
                }
            }
        }
        
    }
}
