using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class FieldGenerator : MonoBehaviour {

    public NavMeshSurface surface;
    public GameObject ground;
	public GameObject wall;
	public GameObject player;
    public Color originalColor;
    public Color targetColor;

    private GeneralNumbers genNumbers;
    private bool updateNavMeshGrid = false;

    public List<GameObject> possibleTargetPoints = new List<GameObject> ();

    void Awake()
    {
        genNumbers = GetComponent<GeneralNumbers>();
    }

    void Start ()
    {
        GenerateGrid();
        GenerateUnits();

        // UPDATE NAVMESH
        surface.BuildNavMesh();
        
    }
	
    void GenerateGrid()
    {
        for (int x = 1; x <= genNumbers.gridSize; x++)
        {
            for (int y = 1; y <= genNumbers.gridSize; y++)
            {
                Vector3 pos = new Vector3(x, 1f, y);
                Instantiate(ground, pos, Quaternion.identity, transform);
            }
        }
    }

    void GenerateUnits()
    {
        for (int num = 1; num <= genNumbers.numberOfUnits; num++)
        {
            Vector3 pos = new Vector3(Random.Range(1, genNumbers.gridSize), 1.25f, Random.Range(1, genNumbers.gridSize));
            Instantiate(player, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        
        if(updateNavMeshGrid)
        {
            surface.BuildNavMesh();
            updateNavMeshGrid = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == ground.tag)
                {
                    // Delete the original target and color
                    MeshRenderer hitMeshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    hitMeshRenderer.material.color = originalColor;
                    possibleTargetPoints.Remove(hit.collider.gameObject);

                    // Put the cube
                    Vector3 pos = new Vector3(hit.collider.gameObject.transform.position.x, 2.0f, hit.collider.gameObject.transform.position.z);
                    Instantiate(wall, pos, Quaternion.identity);
                    updateNavMeshGrid = true;
                }
                else if (hit.collider.gameObject.tag == wall.tag)
                {
                    Destroy(hit.collider.gameObject);
                    updateNavMeshGrid = true;
                }
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == ground.tag)
                {
                    MeshRenderer hitMeshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    if (hitMeshRenderer.material.color == originalColor)
                    {
                        hitMeshRenderer.material.color = targetColor;
                        possibleTargetPoints.Add(hit.collider.gameObject);
                    }
                    else if (hitMeshRenderer.material.color == targetColor)
                    {
                        hitMeshRenderer.material.color = originalColor;
                        possibleTargetPoints.Remove(hit.collider.gameObject);
                    }
                }
            }
        }
        // For debug purpose
        //else if (Input.GetMouseButtonDown(2))
        //{
        //    ChangeGameMode();
        //}
    }

    public void ChangeGameMode()
    {
        if (genNumbers.gameMode == GeneralNumbers.PossibleGameMode.toTarget)
            genNumbers.gameMode = GeneralNumbers.PossibleGameMode.normal;
        else if (genNumbers.gameMode == GeneralNumbers.PossibleGameMode.normal)
            genNumbers.gameMode = GeneralNumbers.PossibleGameMode.toTarget;
    }

}
