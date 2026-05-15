using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [Header("Interact Area")]
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactRadius;
    [SerializeField] private Transform interactPoint;

    I_Interactable intertactedObject;

    //[Space]

    private PlayerInputController inputCtrl;

    void Start()
    {
        inputCtrl = GameManager.Instance.playerInputController;
    }

    public void TryInteract()
    {
        Debug.Log("Etkileşime geçildi 1");
        if(inputCtrl == null) return;
        Debug.Log("Etkileşime geçildi 2");
        if(inputCtrl.CanMove == false) return;
        Debug.Log("Etkileşime geçildi 3");

        Collider2D obj = Physics2D.OverlapCircle(interactPoint.position, interactRadius, interactLayer); 

        if(obj == null) return;
        Debug.Log("Etkileşime geçildi 4");

        if (obj.TryGetComponent(out intertactedObject))
        {
            Debug.Log("Etkileşime geçildi 5");
            intertactedObject.Interact();
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(interactPoint.position, interactRadius);
    }

}
