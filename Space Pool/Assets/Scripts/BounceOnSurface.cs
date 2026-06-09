using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BounceOnSurface : MonoBehaviour
{
    public GameSettings gameSettings;
    private Collider col;

    void Awake()
    {
        if (gameSettings == null)
        {
            Debug.LogError("GameSettings not linked on wall! " + gameObject.name);
            return;
        }

        col = GetComponent<Collider>();

        //check if you assigned a material in the Inspector
        if (col.material == null)
        {
            Debug.LogError("No Physic Material is assigned to the 'Material' slot on the Mesh Collider! " + gameObject.name);
            return;
        }

        col.material.bounciness = gameSettings.wallBounciness;

    }
}