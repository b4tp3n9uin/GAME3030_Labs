using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon Spawn"), SerializeField]
    GameObject Weapon;

    public Sprite crossHair;

    [SerializeField]
    GameObject SocketLocation;

    // Start is called before the first frame update
    void Start()
    {
        GameObject SpawnedWeapon = Instantiate(Weapon, SocketLocation.transform.position, SocketLocation.transform.rotation, SocketLocation.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
