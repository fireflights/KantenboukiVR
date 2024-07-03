using System.Collections.Generic;
using System;
using UnityEngine;

public class StarField : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float starSizeMin = 0f;
    [Range(0, 100)]
    [SerializeField] private float starSizeMax = 5f;
    private List<StarDataLoader.Star> stars;
    private List<GameObject> starObjects;
    private Dictionary<int, GameObject> constellationVisible = new Dictionary<int, GameObject> ();

    private readonly int starFieldScale = 400;

    void Start()
    {
        // Read in the star data.
        StarDataLoader sdl = new StarDataLoader();
        stars = sdl.LoadData();
        starObjects = new List<GameObject>();
        foreach (StarDataLoader.Star star in stars)
        {
            // Create star game objects.
            GameObject stargo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            stargo.transform.parent = transform;
            stargo.name = $"HR {star.catalog_number}";
            stargo.transform.localPosition = star.position * starFieldScale;
            //stargo.transform.localScale = Vector3.one * Mathf.Lerp(starSizeMin, starSizeMax, star.size);
            stargo.transform.LookAt(transform.position);
            stargo.transform.Rotate(0, 180, 0);
            Material material = stargo.GetComponent<MeshRenderer>().material;
            material.shader = Shader.Find("Unlit/StarShader");
            material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, star.size));
            material.color = star.colour;
            starObjects.Add(stargo);
        }
    }

    // Could also do in Update with Time.deltatime scaling.
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, Input.GetAxis("Mouse Y"));
            Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, -Input.GetAxis("Mouse X"));
        }
        return;
    }

    private void OnValidate()
    {
        if (starObjects != null)
        {
            for (int i = 0; i < starObjects.Count; i++)
            {
                // Update the size set in the shader.
                Material material = starObjects[i].GetComponent<MeshRenderer>().material;
                material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, stars[i].size));
            }
        }
    }

    /*// A constellation is a tuple of the stars and the lines that join them.
    private readonly List<(int[], int[])> constellations = new()
    {
        // 2024-7-1 0:00 Shinjuku Area
        //1,2,3(North)
        //4,5,6,7,8,9(West)
        //0,q,w(South)
        //e(East)

        // Cassiopeia (�J�V�I�y����) Press 0
        (new int[] { 21, 168, 264, 403, 542 },
    new int[] { 21, 168, 264, 168, 264, 403, 542, 403 }),
        // Cancer (���ɍ�) Press 1
        (new int[] { 3475, 3449, 3461, 3572, 3249 },
    new int[] { 3475, 3449, 3449, 3461, 3461, 3572, 3461, 3249 }),
        // Ursa Major (�������܍�) Press 2
        (new int[] { 3569, 3594, 3775, 3888, 3323, 3757, 4301, 4295, 4554, 4660,
                4905, 5054, 5191, 4518, 4335, 4069, 4033, 4377, 4375 },
    new int[] { 3569, 3594, 3594, 3775, 3775, 3888, 3888, 3323, 3323, 3757,
                3757, 3888, 3757, 4301, 4301, 4295, 4295, 3888, 4295, 4554,
                4554, 4660, 4660, 4301, 4660, 4905, 4905, 5054, 5054, 5191,
                4554, 4518, 4518, 4335, 4335, 4069, 4069, 4033, 4518, 4377, 4377, 4375 }),
        // Leo (������) Press3
        (new int[] { 3982, 4534, 4057, 4357, 3873, 4031, 4359, 3975, 4399, 4386, 3905, 3773, 3731 },
    new int[] { 4534, 4357, 4534, 4359, 4357, 4359, 4357, 4057, 4057, 4031,
                4057, 3975, 3975, 3982, 3975, 4359, 4359, 4399, 4399, 4386,
                4031, 3905, 3905, 3873, 3873, 3975, 3873, 3773, 3773, 3731, 3731, 3905 }),

        // Corona Borealis (����ނ��) Press 4
        (new int[] { 5971, 5947, 5889, 5849, 5793, 5747, 5778 },
    new int[] { 5971, 5947, 5947, 5889, 5889, 5849, 5849, 5793, 5793, 5747, 5747, 5778 }),


        // Hercules (�w���N���X��) Press 5
        (new int[] { 6588, 6695, 6485, 6418, 6324, 6212, 6220, 6092, 5914, 6148, 6095, 6117, 6406, 6410, 6526, 6703, 6779 },
    new int[] { 6588, 6695, 6695, 6485, 6485, 6418, 6418, 6324, 6324, 6212, 6212, 6220, 6220, 6092, 6092,
                5914, 6212, 6148, 6148, 6095, 6095, 6117, 6148, 6406, 6406, 6410, 6410, 6324, 6410, 6526,
                6526, 6703,6703, 6779 }),

        //Scutum(���č�) Press 6
        (new int[] { 7063, 7032, 7020, 6930, 6973 },
    new int[] { 7063, 7032, 7032, 7020, 7020, 6930, 6930, 6973, 6973, 7063 }),



        // Aquila (�킵��) Press 7
        (new int[] { 7610, 7525, 7557, 7602, 7710, 7429, 7377, 7236, 7235, 7176 },
    new int[] { 7610, 7525, 7525, 7557, 7525, 7235, 7557, 7602,7557,
               7429, 7602, 7710,7377, 7429, 7236, 7377, 7176, 7235,7602,7710}),

        // Cygnus (�͂����傤��) Press 8

        (new int[] { 7924, 7796, 7949, 8115, 7615, 7417, 7528, 7420 },
    new int[] {7924, 7796, 7796, 7949, 7949, 8115, 7796, 7528,
               7528, 7420, 7796, 7615, 7615, 7417}),

        // Cepheus (�P�t�F�E�X��) Press 9
        (new int[] { 8162, 8238, 8974, 8694, 8571 },
    new int[] { 8162, 8571, 8571, 8694, 8694, 8238, 8238, 8162, 8238, 8974, 8974, 8694 }),

        //Triangulum(���񂩂���) Press q
        (new int[] { 664, 622, 544 },
    new int[] { 664, 622, 622, 544, 544, 664 }),

        //Pisces(������) Press w
        (new int[] { 352, 383, 360, 510, 596, 549, 489, 224, 9072, 8969, 8984, 8911, 8852, 8916 },
    new int[] { 352,383,383,360,360,352,360,510,510,596,596,549,549,489,489,224,224,
                9072,9072,8969,8969,8984,8984,8911,8911,8852,8852,8916,8916,8969}),

        // Taurus (��������) Press e
        (new int[] { 1791, 1409, 1346, 1457, 1910, 1239, 1038, 1251, 1030, 1099 },
    new int[] { 1791, 1409, 1409, 1346, 1346, 1457, 1457, 1910, 1346, 1239, 1239, 1038, 1038, 1251, 1038, 1030, 1030, 1099 }),

        // Gemini (�ӂ�����) Press r
        (new int[] { 2890, 2891, 2990, 2421, 2777, 2473, 2650, 2216, 2895,
                2343, 2484, 2286, 2134, 2763, 2697, 2540, 2821, 2905, 2985},
    new int[] { 2890, 2697, 2990, 2905, 2697, 2473, 2905, 2777, 2777, 2650,
                2650, 2421, 2473, 2286, 2286, 2216, 2473, 2343, 2216, 2134,
                2763, 2484, 2763, 2777, 2697, 2540, 2697, 2821, 2821, 2905, 2905, 2985 }),

    };*/

  private readonly List<Tuple<int[], int[]>> constellations = new List<Tuple<int[], int[]>>
  {
    // Cassiopeia (カシオペア座) Press 0
    Tuple.Create(new int[] { 21, 168, 264, 403, 542 }, new int[] { 21, 168, 264, 168, 264, 403, 542, 403 }),
    // Cancer (蟹座) Press 1
    Tuple.Create(new int[] { 3475, 3449, 3461, 3572, 3249 }, new int[] { 3475, 3449, 3449, 3461, 3461, 3572, 3461, 3249 }),
    // Ursa Major (大熊座) Press 2
    Tuple.Create(new int[] { 3569, 3594, 3775, 3888, 3323, 3757, 4301, 4295, 4554, 4660,
            4905, 5054, 5191, 4518, 4335, 4069, 4033, 4377, 4375 },
        new int[] { 3569, 3594, 3594, 3775, 3775, 3888, 3888, 3323, 3323, 3757,
            3757, 3888, 3757, 4301, 4301, 4295, 4295, 3888, 4295, 4554,
            4554, 4660, 4660, 4301, 4660, 4905, 4905, 5054, 5054, 5191,
            4554, 4518, 4518, 4335, 4335, 4069, 4069, 4033, 4518, 4377, 4377, 4375 }),
    // Leo (獅子座) Press 3
    Tuple.Create(new int[] { 3982, 4534, 4057, 4357, 3873, 4031, 4359, 3975, 4399, 4386, 3905, 3773, 3731 },
        new int[] { 4534, 4357, 4534, 4359, 4357, 4359, 4357, 4057, 4057, 4031,
            4057, 3975, 3975, 3982, 3975, 4359, 4359, 4399, 4399, 4386,
            4031, 3905, 3905, 3873, 3873, 3975, 3873, 3773, 3773, 3731, 3731, 3905 }),
    // Corona Borealis (冠座) Press 4
    Tuple.Create(new int[] { 5971, 5947, 5889, 5849, 5793, 5747, 5778 }, new int[] { 5971, 5947, 5947, 5889, 5889, 5849, 5849, 5793, 5793, 5747, 5747, 5778 }),
    // Hercules (ヘルクレス座) Press 5
    Tuple.Create(new int[] { 6588, 6695, 6485, 6418, 6324, 6212, 6220, 6092, 5914, 6148, 6095, 6117, 6406, 6410, 6526, 6703, 6779 },
        new int[] { 6588, 6695, 6695, 6485, 6485, 6418, 6418, 6324, 6324, 6212, 6212, 6220, 6220, 6092, 6092,
            5914, 6212, 6148, 6148, 6095, 6095, 6117, 6148, 6406, 6406, 6410, 6410, 6324, 6410, 6526,
            6526, 6703, 6703, 6779 }),
    // Scutum (盾座) Press 6
    Tuple.Create(new int[] { 7063, 7032, 7020, 6930, 6973 }, new int[] { 7063, 7032, 7032, 7020, 7020, 6930, 6930, 6973, 6973, 7063 }),
    // Aquila (鷲座) Press 7
    Tuple.Create(new int[] { 7610, 7525, 7557, 7602, 7710, 7429, 7377, 7236, 7235, 7176 },
        new int[] { 7610, 7525, 7525, 7557, 7525, 7235, 7557, 7602, 7557,
                7429, 7602, 7710, 7377, 7429, 7236, 7377, 7176, 7235, 7602, 7710 }),
    // Cygnus (白鳥座) Press 8
    Tuple.Create(new int[] { 7924, 7796, 7949, 8115, 7615, 7417, 7528, 7420 },
        new int[] { 7924, 7796, 7796, 7949, 7949, 8115, 7796, 7528,
                7528, 7420, 7796, 7615, 7615, 7417 }),
    // Cepheus (ケフェウス座) Press 9
    Tuple.Create(new int[] { 8162, 8238, 8974, 8694, 8571 },
        new int[] { 8162, 8571, 8571, 8694, 8694, 8238, 8238, 8162, 8238, 8974, 8974, 8694 }),
    // Triangulum (三角座) Press q
    Tuple.Create(new int[] { 664, 622, 544 }, new int[] { 664, 622, 622, 544, 544, 664 }),
    // Pisces (魚座) Press w
    Tuple.Create(new int[] { 352, 383, 360, 510, 596, 549, 489, 224, 9072, 8969, 8984, 8911, 8852, 8916 },
        new int[] { 352, 383, 383, 360, 360, 352, 360, 510, 510, 596, 596, 549, 549, 489, 489, 224, 224,
                9072, 9072, 8969, 8969, 8984, 8984, 8911, 8911, 8852, 8852, 8916, 8916, 8969 }),
    // Taurus (牡牛座) Press e
    Tuple.Create(new int[] { 1791, 1409, 1346, 1457, 1910, 1239, 1038, 1251, 1030, 1099 },
        new int[] { 1791, 1409, 1409, 1346, 1346, 1457, 1457, 1910, 1346, 1239, 1239, 1038, 1038, 1251, 1038, 1030, 1030, 1099 }),
    // Gemini (双子座) Press r
    Tuple.Create(new int[] { 2890, 2891, 2990, 2421, 2777, 2473, 2650, 2216, 2895,
            2343, 2484, 2286, 2134, 2763, 2697, 2540, 2821, 2905, 2985 },
        new int[] { 2890, 2697, 2990, 2905, 2697, 2473, 2905, 2777, 2777, 2650,
            2650, 2421, 2473, 2286, 2286})
        };

    private void Update()
    {
        // Check for numeric presses and toggle the constellation highlighting.
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                ToggleConstellation(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int specialConstellationIndex = 10; // Define the index for the special constellation.
            ToggleConstellation(specialConstellationIndex);
        }
        // Check for the 'b' key press and toggle another specific constellation.
        if (Input.GetKeyDown(KeyCode.W))
        {
            int specialConstellationIndexW = 11; // Define the index for the special constellation 'b'.
            ToggleConstellation(specialConstellationIndexW);
        }

        // Check for the 'c' key press and toggle another specific constellation.
        if (Input.GetKeyDown(KeyCode.E))
        {
            int specialConstellationIndexE = 12; // Define the index for the special constellation 'c'.
            ToggleConstellation(specialConstellationIndexE);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            int specialConstellationIndexR = 13; // Define the index for the special constellation 'c'.
            ToggleConstellation(specialConstellationIndexR);
        }
    }


    void ToggleConstellation(int index)
    {
        // Safety check the index is valid.
        if ((index < 0) || (index >= constellations.Count))
        {
            return;
        }

        // Toggle on or off.
        if (constellationVisible.ContainsKey(index))
        {
            RemoveConstellation(index);
        }
        else
        {
            CreateConstellation(index);
        }
    }

    void CreateConstellation(int index)
    {
        int[] constellation = constellations[index].Item1;
        int[] lines = constellations[index].Item2;

        // Change the colours of the stars
        foreach (int catalogNumber in constellation)
        {
            // Remember list is 0-up catalog numbers are 1-up.
            starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = Color.white;
        }

        GameObject constellationHolder = new GameObject($"Constellation {index}");
        constellationHolder.transform.parent = transform;
        constellationVisible[index] = constellationHolder;

        // Draw the constellation lines.
        for (int i = 0; i < lines.Length; i += 2)
        {
            // Parent it to our constellation object so we can delete them all later.
            GameObject line = new GameObject("Line");
            line.transform.parent = constellationHolder.transform;
            // Defaults to white and width 1 which works for us.
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            // Doesn't get assigned a material so just dig out one that works.
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            // Disable useWorldSpace so it will track the parent game object.
            lineRenderer.useWorldSpace = false;
            Vector3 pos1 = starObjects[lines[i] - 1].transform.position;
            Vector3 pos2 = starObjects[lines[i + 1] - 1].transform.position;
            // Offset them so they don't occlude the stars, 3 chosen by trial and error.
            Vector3 dir = (pos2 - pos1).normalized * 3;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, pos1 + dir);
            lineRenderer.SetPosition(1, pos2 - dir);
        }
    }

    void RemoveConstellation(int index)
    {
        int[] constallation = constellations[index].Item1;

        // Toggling off set the stars back to the original colour.
        foreach (int catalogNumber in constallation)
        {
            // Remember list is 0-up catalog numbers are 1-up.
            starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = stars[catalogNumber - 1].colour;
        }
        // Remove the constellation lines.
        Destroy(constellationVisible[index]);
        // Remove from our dictionary as it's now off.
        constellationVisible.Remove(index);
    }

}

