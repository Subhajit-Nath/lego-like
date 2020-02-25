using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject floorPrefab;

    [SerializeField] int xSize = 10;
    [SerializeField] int zSize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] float spacing = 1f;

    [Space]
    [SerializeField] private Transform[] referenceCubes;
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask cubeMask;
    [SerializeField] private GameObject[] cubePrefabs;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private ColorPicker picker;
    public static bool canSpawn = true;

    private GameObject floorGRP;
    private int slotIndex = 0;
    private Transform referenceCube;
    private GameObject cubePrefab;
    private Color selectedColor;
    private int cubeLayer;
    // private Entity entityPrefab;
    // private World defaultWorld;
    // private EntityManager entityManager;

    private void InstantiatePlane(Vector3 position)
    {
        GameObject plane = Instantiate(floorPrefab, floorGRP.transform);
        plane.transform.position = position;
    }

    private void InstantiateGroundGrid(int dimX, int dimZ, float spacing = 1f)
    {
        floorGRP = new GameObject("Floor Group");
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimZ; j++)
            {
                InstantiatePlane(new Vector3(i * spacing - dimX / 2f, 0f, j * spacing - dimZ / 2f));
            }
        }
    }

    private void Start()
    {
        selectedColor = new Color32(128, 128, 128, 255);
        picker.onValueChanged.AddListener(color =>
        {
            selectedColor = color;
            UIManager.instance.SetSelectedColorSample(selectedColor);
        });
        picker.CurrentColor = selectedColor;
        picker.gameObject.SetActive(false);

        cubeLayer = LayerMask.NameToLayer("CubeLayer");
        InstantiateGroundGrid(xSize, zSize, spacing);
        referenceCubes[0].gameObject.SetActive(true);
        referenceCube = referenceCubes[0];
        cubePrefab = cubePrefabs[0];
        UIManager.instance.UpdateSelectionUI(slotIndex);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            if (scroll > 0)
                slotIndex--;
            else
                slotIndex++;

            if (slotIndex > (referenceCubes.Length - 1))
                slotIndex = 0;
            if (slotIndex < 0)
                slotIndex = (referenceCubes.Length - 1);
            referenceCube = referenceCubes[slotIndex];
            cubePrefab = cubePrefabs[slotIndex];

            if (!referenceCube.gameObject.activeSelf)
            {
                for (int i = 0; i < referenceCubes.Length; i++)
                {
                    if (i == slotIndex)
                    {
                        referenceCubes[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        referenceCubes[i].gameObject.SetActive(false);
                    }
                }
            }
            UIManager.instance.UpdateSelectionUI(slotIndex);
        }

        RaycastHit hit;

        if (Time.timeScale == 1f && Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, rayDistance, cubeMask))
        {
            Transform objectHit = hit.transform;
            float offset = 0;
            if (hit.normal.x != 0)
            {
                offset = objectHit.GetComponent<MeshRenderer>().bounds.extents.x + referenceCube.GetComponent<MeshRenderer>().bounds.extents.x;
            }
            else if (hit.normal.y != 0)
            {
                offset = objectHit.GetComponent<MeshRenderer>().bounds.extents.y + referenceCube.GetComponent<MeshRenderer>().bounds.extents.y;
            }
            else if (hit.normal.z != 0)
            {
                offset = objectHit.GetComponent<MeshRenderer>().bounds.extents.z + referenceCube.GetComponent<MeshRenderer>().bounds.extents.z;
            }
            referenceCube.position = objectHit.position + hit.normal * offset;// + hit.normal * 0.5f;
            //referenceCube.rotation = Quaternion.LookRotation(hit.normal);

            if (Input.GetMouseButtonDown(0) && canSpawn)
            {
                GameObject thisCube = Instantiate(cubePrefab, referenceCube.transform.position, referenceCube.transform.rotation);
                MeshFilter meshFilter = thisCube.GetComponent<MeshFilter>();
                Mesh mesh = meshFilter.mesh;
                //Vector3[] vertices = mesh.vertices;
                Color[] colors = new Color[mesh.vertexCount];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = selectedColor;// new Color32(200, 200, 100, 255);
                }
                mesh.colors = colors;
                meshFilter.mesh = mesh;
            }
            else if (Input.GetMouseButtonDown(1) && objectHit.gameObject.layer == cubeLayer)
            {
                Destroy(objectHit.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UIManager.instance.ToggleColorPicker();
        }
    }


    /*
        void Start()
        {
            defaultWorld = World.DefaultGameObjectInjectionWorld;
            entityManager = defaultWorld.EntityManager;

            if (floorPrefab != null)
            {
                GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
                entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(floorPrefab, settings);

                InstantiateEntityGrid(xSize, zSize, spacing);
            }
        }

        private void InstantiateEntity(float3 position)
        {
            if (entityManager == null)
            {
                Debug.LogWarning("InstantiateEntity WARNING: No EntityManager found!");
                return;
            }

            Entity myEntity = entityManager.Instantiate(entityPrefab);
            entityManager.SetComponentData(myEntity, new Translation
            {
                Value = position
            });
        }

        private void InstantiateEntityGrid(int dimX, int dimZ, float spacing = 1f)
        {
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimZ; j++)
                {
                    InstantiateEntity(new float3(i * spacing - dimX / 2f, 0f, j * spacing - dimZ / 2f));
                }
            }
        }
    */

}