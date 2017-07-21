using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMotion : MonoBehaviour
{
    public GameObject mc;
    public float speed = 2.5f;
    public float jumpPower = 0.3f;
    public float gunRange = 20f;
    public float gunPower = 1f;
    public float bulletAmount = 30f;
    public float totalAmmo = 120f;
    public AudioClip shootingClip;
    public AudioClip noAmmoClip;
    public AudioClip reloadClip;

    private Animator anim;
    private IEnumerator shootContinue;
    private ParticleSystem muzzle;
    private float shootingSpeed = 1f;
    private float sideSpeed = 0.8f;
    private float backSpeed = 0.6f;
    private float shootingTime = 0f;
    private float currentBullet = 0f;
    private bool inJump = false;
    private bool inReload = false;
    private Vector3 shootStartPostion;

    // Use this for initialization
    void Start()
    {
        currentBullet = bulletAmount;
        anim = GetComponent<Animator>();
        muzzle = GetComponentInChildren<ParticleSystem>();
        muzzle.Stop();
        anim.SetBool("Stand", true);
    }

    // Update is called once per frame
    void Update()
    {
        // 人物跟著鏡頭
        if (Input.GetAxis("Mouse X") != 0)
        {
            transform.rotation = Quaternion.LookRotation(vector(mc.transform.position, transform.position));
        }

        // 點擊滑鼠進入射擊狀態
        if (Input.GetMouseButtonDown(0) && !inReload)
        {
            if (currentBullet > 0f)
            {
                anim.SetTrigger("Attack");
                shootingSpeed = 0.8f;
                mc.GetComponent<AudioSource>().loop = true;
                mc.GetComponent<AudioSource>().Play();
                shootContinue = shooting();
                muzzle.Play();
                StartCoroutine(shootContinue);
            }
            else
            {
                mc.GetComponent<AudioSource>().Play();
            }
        }
        if (Input.GetMouseButtonUp(0) && !inReload)
        {
            anim.speed = 1f;
            shootingSpeed = 1f;
            mc.GetComponent<AudioSource>().Stop();
            muzzle.Stop();
            StopCoroutine(shootContinue);
        }

        if (Input.GetKeyDown(KeyCode.R) && !inReload && totalAmmo > 0f)
        {
            muzzle.Stop();
            StopCoroutine(shootContinue);
            StartCoroutine(reload());
        }

        // 偵測是否有按任何關於移動的按鍵，有就改變動畫狀態
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Stand", false);
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Stand", false);
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Stand", false);
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Stand", false);
            }
            else
            {
                anim.SetBool("Run", false);
                anim.SetBool("Stand", true);
            }
        }

        // 有按WASD則移動
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            anim.speed = shootingSpeed;
            transform.position += transform.forward * speed * shootingSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            anim.speed = backSpeed * shootingSpeed;
            transform.position += transform.forward * -1f * speed * backSpeed * shootingSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.speed = sideSpeed * shootingSpeed;
            transform.position += transform.right * -1f * speed * sideSpeed * shootingSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            anim.speed = sideSpeed * shootingSpeed;
            transform.position += transform.right * speed * sideSpeed * shootingSpeed * Time.deltaTime;
        }

        // 偵測是否按空白鍵跳躍
        if (Input.GetKeyDown(KeyCode.Space) && !inJump)
        {
            StartCoroutine(jump());
        }
    }

    public float getCurrentBullet()
    {
        return currentBullet;
    }

    Vector3 vector(Vector3 from, Vector3 to)
    {
        return new Vector3(to.x - from.x, 0f, to.z - from.z);
    }

    IEnumerator jump()
    {
        inJump = true;
        float count = jumpPower;
        while (count > 0)
        {
            transform.position += new Vector3(0f, count, 0f);
            count -= Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        inJump = false;
    }

    IEnumerator shooting()
    {
        RaycastHit hit;
        while (true)
        {
            shootStartPostion = transform.position;
            shootStartPostion += transform.forward * 0.8f;
            shootStartPostion += transform.up * 1.2f;
            shootStartPostion += transform.right * 0.175f;

            Ray shootRay = new Ray(shootStartPostion, transform.forward);
            currentBullet--;
            if (currentBullet == 0f)
            {
                anim.speed = 1f;
                shootingSpeed = 1f;
                muzzle.Stop();
                mc.GetComponent<AudioSource>().Stop();
                mc.GetComponent<AudioSource>().clip = noAmmoClip;
                mc.GetComponent<AudioSource>().loop = false;
                mc.GetComponent<AudioSource>().Play();
                yield break;
            }
            if (Physics.Raycast(shootRay, out hit, gunRange))
            {
                if (hit.transform.tag.Equals("Monster"))
                {
                    hit.transform.GetComponent<MonsterHealth>().beAttacked(gunPower);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator reload()
    {
        inReload = true;
        if (totalAmmo >= bulletAmount)
        {
            totalAmmo -= (bulletAmount - currentBullet);
            currentBullet = bulletAmount;
        }
        else
        {
            currentBullet = totalAmmo;
            totalAmmo = 0f;
        }
        mc.GetComponent<AudioSource>().clip = reloadClip;
        mc.GetComponent<AudioSource>().loop = false;
        mc.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(mc.GetComponent<AudioSource>().clip.length);
        mc.GetComponent<AudioSource>().clip = shootingClip;
        inReload = false;
    }
}
