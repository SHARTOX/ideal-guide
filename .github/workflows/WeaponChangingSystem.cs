using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingWeapon : MonoBehaviour
{
    public GameObject charSc;
    public int selectedWeapon = 0;
    CharacterMovement _sc;

    // Start is called before the first frame update
    void Start()
    {
        _sc = charSc.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f && _sc.isSprinting == false)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
            selectedWeapon++;
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f && _sc.isSprinting == false)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
            selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }
        
    }

    void SelectedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
