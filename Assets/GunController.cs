using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public float rateOfFire;
    public float currentAmmo, equippedAmmo;
    public float clipSize;
    public float maxAmmo;
    public bool reloading;
    public float bulletVelocity = 20f;
    private bool canFire = true;
    [SerializeField] TextMeshProUGUI curAmmo, equipAmmo, maxClipSize;
    [SerializeField] ParticleSystem fireAnim;
    [SerializeField] Rigidbody bullet;
    [SerializeField] GameObject bulPos;
    [SerializeField] GameObject ammoPanel;

    private void Start() {
        curAmmo.text = currentAmmo.ToString();
        equipAmmo.text = equippedAmmo.ToString();
        maxClipSize.text = clipSize.ToString();
    }

    // Update is called once per frame
    void Update() {

        if (GameManager.instance.OptionsPanelActive() == false) {
            ammoPanel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R)) {
                GetComponent<Animator>().SetTrigger("reloading");
                if (equippedAmmo == clipSize) {
                }
                if (equippedAmmo < clipSize) {
                    if (currentAmmo >= (clipSize - equippedAmmo)) {
                        currentAmmo = currentAmmo + (equippedAmmo - clipSize);
                        curAmmo.text = currentAmmo.ToString();
                        equippedAmmo = clipSize;
                        equipAmmo.text = equippedAmmo.ToString();
                    } else {
                        equippedAmmo += currentAmmo;
                        currentAmmo = 0;
                        equipAmmo.text = equippedAmmo.ToString();
                        curAmmo.text = currentAmmo.ToString();
                    }
                }
                //GetComponent<Animator>().SetBool("isReloading", false);
            }
            if (Input.GetMouseButtonDown(0) && canFire) {
                if (equippedAmmo > 0) {
                    GetComponent<Animator>().SetTrigger("firing");
                    equippedAmmo -= 1;
                    StartCoroutine(Fire());
                    equipAmmo.text = equippedAmmo.ToString();
                }
                //GetComponent<Animator>().SetBool("isFiring", false);
            }
        } else {
            ammoPanel.SetActive(false);
        }
    }

    IEnumerator Fire() {
        canFire = false;
       
        Rigidbody bulletClone = (Rigidbody) Instantiate(bullet, bulPos.transform.position, bulPos.transform.rotation);
        bulletClone.velocity = transform.forward * bulletVelocity;
        bulletClone.gameObject.SetActive(true);

        fireAnim.Play();
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }

}
