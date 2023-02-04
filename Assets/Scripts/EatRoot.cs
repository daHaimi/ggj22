using System.Collections;
using UnityEngine;

public class EatRoot : MonoBehaviour
{
    public GameObject effectFire;

    public void OnEnable()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Root")) return;
        Destroy(collision.gameObject);
        effectFire.SetActive(true);
        StartCoroutine(HideFireCoroutine());
    }

    IEnumerator HideFireCoroutine()
    {
        yield return new WaitForSeconds(.3f);
        effectFire.SetActive(false);
    }
}
