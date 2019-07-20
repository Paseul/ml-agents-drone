using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using MLAgents;

public class PyramidAgent : Agent
{
    public GameObject area;
    private PyramidArea myArea;
    private Rigidbody agentRb;
    private RayPerception rayPer;
    private PyramidSwitch switchLogic;
    public GameObject areaSwitch;
    public bool useVectorObs;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentRb = GetComponent<Rigidbody>();
        myArea = area.GetComponent<PyramidArea>();
        rayPer = GetComponent<RayPerception>();
        switchLogic = areaSwitch.GetComponent<PyramidSwitch>();
    }

    public override void CollectObservations()
    {
        if (useVectorObs)
        {
            const float rayDistance = 35f;
            float[] rayAngles = {20f, 90f, 160f, 45f, 135f, 70f, 110f};
            float[] rayAngles1 = {25f, 95f, 165f, 50f, 140f, 75f, 115f};
            float[] rayAngles2 = {15f, 85f, 155f, 40f, 130f, 65f, 105f};

            string[] detectableObjects = {"block", "wall", "goal", "stone", "tree", "people" };
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, 5f));
            AddVectorObs(switchLogic.GetState());
            AddVectorObs(transform.InverseTransformDirection(agentRb.velocity));
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, -5f));
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, -10f));
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, -15f));
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 10f));
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 15f));
        }
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(act[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(act[1], -1f, 1f);
        }
        else
        {
            var action = Mathf.FloorToInt(act[0]);
            switch (action)
            {
                case 1:
                    dirToGo = transform.forward * 1f;
                    break;
                case 2:
                    dirToGo = transform.forward * -1f;
                    break;
                case 3:
                    rotateDir = transform.up * 1f;
                    break;
                case 4:
                    rotateDir = transform.up * -1f;
                    break;
                case 5:
                    dirToGo = transform.up * -0.1f;
                    //rotateDir = new Vector3(1f, 0f, 0f);
                    break;
                case 6:
                    dirToGo = transform.up * 0.1f;
                    //rotateDir = new Vector3(-1f, 0f, 0f);
                    break;
            }
        }
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        agentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(-1f / agentParameters.maxStep);

        /*   int layerMask = 1 << 14;

           if (Physics.Raycast(agentRb.position, Vector3.down, 10f, layerMask))
           {
               agentRb.AddForce(Vector3.down * 20f, ForceMode.VelocityChange);
           }*/
        MoveAgent(vectorAction);
        
    }

    public override void AgentReset()
    {
        var enumerable = Enumerable.Range(0, 9).OrderBy(x => Guid.NewGuid()).Take(9);
        var items = enumerable.ToArray();
        
        myArea.CleanPyramidArea();
        
        agentRb.velocity = Vector3.zero;
        myArea.PlaceObject(gameObject, items[1]);
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

        switchLogic.ResetSwitch(items[0], items[0]);
            
        for (var i = 1; i < 9; i++)
        {
            myArea.CreatePeople(1, items[i]);
            myArea.CreateTree(1, items[i]);
            myArea.CreateSkinnyTree(2, items[i]);
            myArea.CreateBush(1, items[i]);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("goal"))
        {
            SetReward(2f);
            Done();
        }
        else if (collision.gameObject.CompareTag("wall"))
        {
            SetReward(-1f);
            Done();
        }
    }

    public override void AgentOnDone()
    {

    }
}
