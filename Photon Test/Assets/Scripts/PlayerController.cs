using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [HideInInspector]
    public int id;

    [Header("Stats")]
    public float moveSpeed = 5;

    private Rigidbody rb;
    public Player photonPlayer;
    private Statusmanager statusManager;

    [Header("Gun")]

    public GameObject bulletPrefab;
    public GameObject bulletLinePrefab;
    public GameObject fireLight;
    public GameObject bulletHolePrefab;

    public LayerMask layerMask;
    public LayerMask bulletholeLayers;
    public Transform gunBarrel;
    private int currentAmmo = 6;
    public int maxAmmo = 6;
    private float reloadDelay = 0.34f;

    public bool isReloading;

    [Header("UI")]
    public TextMeshProUGUI hpbar;
    public TextMeshProUGUI ammo;
    public CinemachineVirtualCamera cinemachine;

    [Header("Sound")]
    public AudioClip gunfire;
    public AudioClip reloadSound;

    [Header("Interaction")]
    public Interactor interactor;


    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        GameManager.instance.players[id - 1] = this;
        rb = GetComponent<Rigidbody>();
        GameManager.instance.SpawnNamePlate(this);
        statusManager = GetComponent<Statusmanager>();
        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }
        else
        {
            cinemachine = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineVirtualCamera>();
            cinemachine.Follow = transform;
            cinemachine.LookAt = transform;
            hpbar = GameObject.Find("HpBar").GetComponent<TextMeshProUGUI>();
            ammo = GameObject.Find("Ammo").GetComponent<TextMeshProUGUI>();
            statusManager.onHpUpdate.AddListener(UpdateHealthBar);
            UpdateHealthBar();

        }
    }

    public void UpdateHealthBar()
    {
        hpbar.text = "Health: " + statusManager.hp + " / " + statusManager.maxHp;
    }

    public void ShakeCamera(float seconds)
    {
        if (!photonView.IsMine)
            return;

        cinemachine.GetComponent<CameraEffects>().ShakeCamera(seconds);
    }
    private void Start()
    {
        /*
        rb = GetComponent<Rigidbody>();
        cinemachine = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = transform;
        cinemachine.LookAt = transform;
        */



    }
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if(Input.GetMouseButtonDown(0) && CurrentAmmo > 0)
        {
            photonView.RPC("Shoot",RpcTarget.All,transform.position, transform.eulerAngles,transform.forward);
        }

        if(Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            photonView.RPC("Reload", RpcTarget.All);
        }
        RotatePlayerToCursor();
        Move();

        if(Input.GetKeyDown(KeyCode.E))
        {
            interactor.Interact();
        }

        if(transform.position.y < -100)
        {
            transform.position = GameManager.instance.spawnPoints[0].position;
        }

    }

    [PunRPC]
    private void Reload()
    {
        isReloading = true;
        if (photonView.IsMine)
        {
            InvokeRepeating("ReloadWeapon", reloadDelay * 1.25f, reloadDelay);;
        }
    }

    private void ReloadWeapon()
    {
        CurrentAmmo++;
        AudioManager.instance.PlaySound(reloadSound, transform.position);
        if(CurrentAmmo >= maxAmmo)
        {
            CancelInvoke("ReloadWeapon");
        }
    }

    private void Move()
    {
        float horrizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horrizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed);
    }

    [PunRPC]
    private void Shoot(Vector3 pos, Vector3 rotation,Vector3 forward)
    {
        isReloading = false;
        CancelInvoke("ReloadWeapon");
        List<RaycastHit> raycastHits = new List<RaycastHit>();
        Dictionary<Statusmanager,int> hitObjects = new Dictionary<Statusmanager, int>();
        for (int i = 0; i < 5;i++)
        {
            RaycastHit raycastHit;
            Ray ray = new Ray(transform.position, transform.forward + new Vector3(Random.Range(-0.15f, 0.15f), Random.Range(-0.05f, 0.05f), Random.Range(-0.15f, 0.15f)));
            Physics.Raycast(ray, out raycastHit,20, layerMask);
            if(raycastHit.collider == null)
            {
                raycastHit.point = ray.GetPoint(10);
            }
            raycastHits.Add(raycastHit);
            
            if(photonView.IsMine && raycastHit.collider != null && raycastHit.collider.gameObject.GetComponent<Statusmanager>() != null)
            {
                Statusmanager sm = raycastHit.collider.gameObject.GetComponent<Statusmanager>();
                if(sm.faction != statusManager.faction)
                {
                    if (hitObjects.ContainsKey(sm))
                    {
                        hitObjects[sm]++;
                    }
                    else
                    {
                        hitObjects.Add(sm, 1);
                    }
                }
            }
        }

        if (photonView.IsMine)
        {
            foreach (Statusmanager sm in hitObjects.Keys)
            {
                sm.TakeDamage(hitObjects[sm],gameObject);
            }
            ShakeCamera(0.02f);
        }



        foreach(RaycastHit rc in raycastHits)
        {
            GameObject bulletLineObj = Instantiate(bulletLinePrefab, Vector3.zero, Quaternion.identity);
            LineRenderer lr = bulletLineObj.GetComponent<LineRenderer>();
            lr.SetPosition(0, gunBarrel.transform.position);
            lr.SetPosition(1, rc.point);
            if(rc.collider != null)
            {
                if(bulletholeLayers == (bulletholeLayers | (1 << rc.collider.gameObject.layer)))
                {
                    GameObject bulletHoleObj = Instantiate(bulletHolePrefab, rc.point + rc.normal * 0.01f, Quaternion.identity);
                    bulletHoleObj.transform.LookAt(rc.point);
                }

            }
            
            Destroy(bulletLineObj, 2);
        }
        AudioManager.instance.PlaySound(gunfire, transform.position);
        fireLight.SetActive(true);
        CancelInvoke("DisableFilreight");
        Invoke("DisableFilreight", Time.fixedDeltaTime*2);
        CurrentAmmo--;
        if(CurrentAmmo == 0)
        {
            photonView.RPC("Reload", RpcTarget.All);
        }
    }

    public void DisableFilreight()
    {
        fireLight.SetActive(false);
    }

    private void RotatePlayerToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero + new Vector3(0,transform.position.y,0));
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    public int CurrentAmmo 
    { 
        get => currentAmmo;
        set 
        {
            currentAmmo = value;
            UpdateAmmoText();
        
        }
    }

    private void UpdateAmmoText()
    {
        if(photonView.IsMine)
        {
            ammo.text = "Ammo: " + currentAmmo + " / " + maxAmmo;
        }
    }

    public void PlayerDeath()
    {
        transform.position = GameManager.instance.spawnPoints[0].position;
        statusManager.Hp = statusManager.maxHp;
    }
}
