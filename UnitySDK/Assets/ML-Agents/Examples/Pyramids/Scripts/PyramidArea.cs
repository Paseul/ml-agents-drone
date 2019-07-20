using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PyramidArea : Area
{
    public GameObject pyramid;
    public GameObject stonePyramid;
    public GameObject tree;
    public GameObject skinnyTree;
    public GameObject bush;
    public GameObject people;
    public GameObject[] spawnAreas;
    public int numPyra;
    public float range;

    public void CreatePyramid(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, pyramid, spawnAreaIndex);
    }
    
    public void CreateStonePyramid(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, stonePyramid, spawnAreaIndex);
    }

    public void CreateTree(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, tree, spawnAreaIndex);
    }

    public void CreateSkinnyTree(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, skinnyTree, spawnAreaIndex);
    }

    public void CreateBush(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, bush, spawnAreaIndex);
    }

    public void CreatePeople(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, people, spawnAreaIndex);
    }

    private void CreateObject(int numObjects, GameObject desiredObject, int spawnAreaIndex)
    {
        for (var i = 0; i < numObjects; i++)
        {
            var newObject = Instantiate(desiredObject, Vector3.zero, 
                Quaternion.Euler(0f, 0f, 0f), transform);
            PlaceObject(newObject, spawnAreaIndex);
        }
    }

    public void PlaceObject(GameObject objectToPlace, int spawnAreaIndex)
    {
        var spawnTransform = spawnAreas[spawnAreaIndex].transform;
        var xRange = spawnTransform.localScale.x / 2.1f;
        var zRange = spawnTransform.localScale.z / 2.1f;
        
        objectToPlace.transform.position = new Vector3(Random.Range(-xRange, xRange), 1f, Random.Range(-zRange, zRange)) 
                                            + spawnTransform.position;
        if(objectToPlace.CompareTag("tree"))
        {
            objectToPlace.transform.position += new Vector3(0f, 4f, 0f);
            objectToPlace.transform.Rotate(Vector3.right, -90f);
        }
        if (objectToPlace.CompareTag("people"))
        {
            objectToPlace.transform.position += new Vector3(0f, -1f, 0f);
         }
        else if (objectToPlace.CompareTag("agent"))
        {
            objectToPlace.transform.position += new Vector3(0f, 4f, 0f);
        }
    }

    public void CleanPyramidArea()
    {
        foreach (Transform child in transform) if (child.CompareTag("stone") || child.CompareTag("tree") || child.CompareTag("people"))
            {
            Destroy(child.gameObject);
        }
    }

    public override void ResetArea()
    {
        
    }
}
