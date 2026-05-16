using JetBrains.Annotations;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
