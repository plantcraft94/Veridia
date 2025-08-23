using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float damage;
    public float ExplodeTimer;
    float currentTimer = 0;
    [SerializeField] GameObject ExplodeEffect;
    public float ColorchangeSpeed;
    Renderer r;
    MaterialPropertyBlock propertyBlock; // Reuse this single instance

    private void Start()
    {
        r = GetComponent<Renderer>();
        
        // Create MaterialPropertyBlock ONCE and reuse it
        propertyBlock = new MaterialPropertyBlock();// Don't forget to apply it!
    }

    private void FixedUpdate()
    {
        currentTimer += Time.fixedDeltaTime;
        
        if (currentTimer >= ExplodeTimer - 1.25f)
        {
            // Only create the color effect during the final countdown
            float t = Mathf.PingPong(Time.time * ColorchangeSpeed, 1f);
            Color ccolor = Color.Lerp(new Color(0.153f, 0.188f, 0.322f, 1.000f), new Color(1.000f, 0.000f, 0.000f, 1.000f), t);
            
            // Reuse the existing propertyBlock instead of creating new ones
            r.GetPropertyBlock(propertyBlock, 0);
            propertyBlock.SetColor("_BaseColor", ccolor);
            r.SetPropertyBlock(propertyBlock, 0);
            
            if (currentTimer >= ExplodeTimer)
            {
                Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}