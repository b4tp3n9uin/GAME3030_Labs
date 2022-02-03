using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47_Script : WeaponComponent
{
    protected override void FireWeapon()
    {
        Vector3 HitLocation;

        if (weaponStats.bulletsInClip > 0 && !isReloading && !weaponHolder.playerController.isRunning)
        {
            base.FireWeapon();

            RaycastHit hit;


            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, weaponStats.fireDistance))
            {
                Debug.Log(hit.transform.name);
                Vector3 hitDirection = hit.point - mainCam.transform.position;
                Debug.DrawRay(mainCam.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 1.5f);
            }


            //Ray screenRay = mainCam.ViewportPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            //if (Physics.Raycast(screenRay, out RaycastHit hit, weaponStats.fireDistance, weaponStats.WeaponHitLayers))
            //{
            //    HitLocation = hit.point;
            //    Vector3 hitDirection = hit.point - mainCam.transform.position;
            //    Debug.DrawRay(mainCam.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 1.5f);
            //}
        }
        else if (weaponStats.bulletsInClip <= 0)
        {
            // Trigger a reload when no bullets left.
            weaponHolder.StartReloading();
        }
    }
}
