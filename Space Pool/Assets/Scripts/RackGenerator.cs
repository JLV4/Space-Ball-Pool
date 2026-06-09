using UnityEngine;

public class RackGenerator : MonoBehaviour
{
    public GameObject ballPrefab;
    public Vector3 rackCenter = new Vector3(0, 1000f, 0); // center of the arrangement
    public float spacing = 100f; // distance between balls
    public Material stripesMaterial;
    public Material solidsMaterial;
    public Material blackMaterial;

    void Start()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("Ball Prefab is not set in the Inspector!");
            return;
        }

        // stripesMaterial = Resources.Load("stripesMaterial", typeof(Material)) as Material;
        // solidsMaterial = Resources.Load("solidsMaterial", typeof(Material)) as Material;
        // blackMaterial = Resources.Load("BlackHoleBall", typeof(Material)) as Material;

        GenerateRack();
    }

    void GenerateRack()
    {
        int ballCount = 1;

        // Layer 1: 1 ball (top)
        CreateBall(rackCenter + Vector3.up * spacing, ballCount++);

        // Layer 2: 3 balls
        Vector3 layer2Center = rackCenter + Vector3.up * (spacing / 2f);
        CreateBall(layer2Center + new Vector3(-spacing, 0, -spacing), ballCount++);
        CreateBall(layer2Center + new Vector3(0, 0, spacing), ballCount++);
        CreateBall(layer2Center + new Vector3(spacing, 0, -spacing), ballCount++);

        // Layer 3: 5 balls (middle layer)
        Vector3 layer3Center = rackCenter;
        float offset = spacing * 1.2f;
        CreateBall(layer3Center + new Vector3(-offset, 0, -offset), ballCount++);
        CreateBall(layer3Center + new Vector3(-offset / 2f, 0, 0), ballCount++);
        CreateBall(layer3Center + new Vector3(0, 0, offset), ballCount++);
        CreateBall(layer3Center + new Vector3(offset / 2f, 0, 0), ballCount++);
        CreateBall(layer3Center + new Vector3(offset, 0, -offset), ballCount++);

        // Layer 4: 6 balls (bottom layer)
        Vector3 layer4Center = rackCenter + Vector3.down * spacing;
        float bottomOffset = spacing * 1.5f;
        CreateBall(layer4Center + new Vector3(-bottomOffset, 0, -bottomOffset), ballCount++);
        CreateBall(layer4Center + new Vector3(-bottomOffset / 2f, 0, -bottomOffset / 2f), ballCount++);
        CreateBall(layer4Center + new Vector3(0, 0, 0), ballCount++);
        CreateBall(layer4Center + new Vector3(bottomOffset / 2f, 0, -bottomOffset / 2f), ballCount++);
        CreateBall(layer4Center + new Vector3(bottomOffset, 0, -bottomOffset), ballCount++);
        CreateBall(layer4Center + new Vector3(0, 0, bottomOffset), ballCount++);
    }

    void CreateBall(Vector3 position, int index)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.name = $"Ball_{index}";
        ball.tag = "Ball";

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = ball.AddComponent<Rigidbody>();
        }

        rb.useGravity = false; // explicitly disable gravity
        rb.isKinematic = false; // allow physics interactions


        if(index == 7)
        {
            ball.GetComponent<Renderer>().material = blackMaterial;
        }
        else if(index % 2 == 0)
        {
            ball.GetComponent<Renderer>().material = solidsMaterial;
        }
        else
        {
            ball.GetComponent<Renderer>().material = stripesMaterial;
        }
    }
}
