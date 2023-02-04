using System.Collections;
using UnityEngine;

public class DropRoot : MonoBehaviour
{
    public MeshRenderer receivingRenderer;
    public GameObject effectFire;

    private Material rendererMat;
    private Texture droppedTexture;

    public void OnEnable()
    {
        if (receivingRenderer == null) return;
        rendererMat = receivingRenderer.material;
        receivingRenderer.sharedMaterial = rendererMat;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Root")) return;
        Destroy(collision.gameObject);
        effectFire.SetActive(true);
        rendererMat.color = Random.ColorHSV();
        StartCoroutine(HideFireCoroutine());
    }

    IEnumerator HideFireCoroutine()
    {
        yield return new WaitForSeconds(.3f);
        effectFire.SetActive(false);
    }
}
