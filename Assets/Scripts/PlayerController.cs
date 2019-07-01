using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    private GeneralNumbers genNumbers;
    private FieldGenerator fieldData;

    private List<GameObject> currentFreePoints = new List<GameObject>();
    private GameObject pointDestination;

    private float bestDistance = 1000f;
    private bool inHouse = false;

    void Awake()
    {
        GameObject GameManager = GameObject.Find("GameManager");
        genNumbers = GameManager.GetComponent<GeneralNumbers>();
        fieldData = GameManager.GetComponent<FieldGenerator>();
    }

    void Update()
    {
        if (!agent.hasPath && genNumbers.gameMode == GeneralNumbers.PossibleGameMode.normal)
        {
            Vector3 newPath = new Vector3(Random.Range(1, genNumbers.gridSize), 1, Random.Range(1, genNumbers.gridSize));
            agent.SetDestination(newPath);

            pointDestination = null;
            inHouse = false;
            bestDistance = 1000f;
        }
        else if(genNumbers.gameMode == GeneralNumbers.PossibleGameMode.toTarget)
        {
            currentFreePoints = fieldData.possibleTargetPoints;

            if (inHouse)
            {
                return;
            }
            else if (pointDestination != null && !currentFreePoints.Contains(pointDestination))
            {
                // Someone has already taken
                pointDestination = null;
                bestDistance = 1000f;
            }
            else if (pointDestination != null && Vector3.Distance(CorrectPointDestination(pointDestination.transform.position), agent.transform.position) < 0.05)
            {
                // Victory
                MeshRenderer hitMeshRenderer = pointDestination.GetComponent<MeshRenderer>();
                hitMeshRenderer.material.color = fieldData.originalColor;
                currentFreePoints.Remove(pointDestination);
                inHouse = true;

            }
            else if (pointDestination == null && currentFreePoints.Count > 0)
            {
                // throwing all looking for a chair
                agent.ResetPath();

                foreach (GameObject possiblePosition in currentFreePoints)
                {
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(possiblePosition.transform.position, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        // Evaluate path
                        float evaluateDistance = Vector3.Distance(agent.transform.position, possiblePosition.transform.position);
                        if (evaluateDistance < bestDistance)
                        {
                            bestDistance = evaluateDistance;
                            pointDestination = possiblePosition;
                        }
                    }
                }

                if (pointDestination != null)
                    agent.SetDestination(CorrectPointDestination(pointDestination.transform.position));
                    
                //else
                    //Debug.Log(this.name + ": can`t found a short path");
            }
            else if (pointDestination == null && currentFreePoints.Count == 0)
            {
                this.gameObject.SetActive(false);
                //Debug.Log(this.name + ": free point not exist");
            }
        }
    }

    Vector3 CorrectPointDestination(Vector3 pos)
    {
        return new Vector3(pos.x, 2f, pos.z);
    }
}
