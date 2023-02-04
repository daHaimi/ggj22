using UnityEngine;

public class BeetController : MonoBehaviour
{
    public void StartDragging()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
