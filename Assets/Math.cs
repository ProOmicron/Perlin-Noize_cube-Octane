using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math : MonoBehaviour
{
    public GameObject prefab;
    public Transform startpivot;
    GameObject[,,] gameObjects;
    public Vector3Int count = new Vector3Int(10, 10, 10);
    public float PerlinScale = 0.75f;
    public float PerlinOffset = 1f;
    public float scaleY = 1;
    public float onoff = 0.2f;

    public AnimationCurve curve;
    private float noizeDirTarget = 10f;
    private float noizeSpeed = 0.1f;
    private float time = 0;
    void Start()
    {
        gameObjects = new GameObject[count.x, count.y, count.z];
        for (int x = 0; x < count.x; x++)
        {
            for (int y = 0; y < count.y; y++)
            {
                for (int z = 0; z < count.z; z++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
                    obj.transform.position = new Vector3(startpivot.position.x + x - (count.x / 2f), startpivot.position.y + y, startpivot.position.z + z - (count.z / 2f));
                    gameObjects[x, y, z] = obj;
                }
            }
        }
        //StartCoroutine(NoizeDirection());
    }

    void Update()
    {
        //PerlinOffset = Mathf.MoveTowards(PerlinOffset, noizeDirTarget, Time.deltaTime * noizeSpeed);
        time += Time.deltaTime * noizeSpeed;
        PerlinOffset = curve.Evaluate(time);

        for (int x = 0; x < count.x; x++)
        {
            for (int y = 0; y < count.y; y++)
            {
                for (int z = 0; z < count.z; z++)
                {
                    var pos = gameObjects[x, y, z].transform.position;
                    var noize = Noize2(pos.x, pos.y, pos.z, PerlinOffset, PerlinScale);
                    if (noize > onoff)
                    {
                        gameObjects[x, y, z].GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        gameObjects[x, y, z].GetComponent<MeshRenderer>().enabled = false;
                    }
                    //gameObjects[x, y, z].transform.localScale = Vector3.one * Noize(x * PerlinScale, y * PerlinScale, z * PerlinScale);
                }
            }
        }
    }

    private IEnumerator NoizeDirection()
    {
        while (true)
        {
            noizeSpeed = Random.Range(0.1f, 0.2f);
            noizeDirTarget = Random.Range(-21f, 17f);
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        }
    }


    private float Noize(float x, float y, float z, float offset, float scale)
    {
        x = (x * scale) + offset;
        y = (y * scale) + offset;
        z = (z * scale) + offset;

        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);

        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        float ABC = AB + BC + AC + BA + CB + CA;
        return ABC / 6f * scaleY;
    }

    private float Noize2(float x, float y, float z, float offset, float scale)
    {
        x = (x * scale) + offset;
        y = (y * scale) + offset;
        z = (z * scale) + offset;

        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6 * scaleY;
    }
}

