using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public Material material;
    public GameObject player;
    public GameObject player2;
    public Vector4[] positions;
    public float range = 10;
    public int FogOfWarBreakersArraySize = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positions = new Vector4[16];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < FogOfWarBreakersArraySize; i++) 
        {
            if (i == 0)
            {
                Vector3 pos = player.transform.position;
                positions[i] = new Vector4(pos.x, range, pos.z, 0);
            }
            else
            {
                Vector3 pos = player2.transform.position;
                positions[i] = new Vector4(pos.x, range, pos.z, 0);

            }
        }
        material.SetVectorArray("_FogOfWarBreakers", positions);
        material.SetTextureOffset("_MainTex", new Vector2(Mathf.Sin(Time.time), Mathf.Cos(Time.time * 3)));
    }
}
