using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KFKPlayerAgent : Agent
{
    Player thisPlayer;

    Player Opponent;

    Rigidbody2D Ball;

    AIPlayerController controller;


    public override void Initialize()
    {
        foreach (var player in FindObjectsOfType<Player>())
        {
            if (player.transform == this.transform.parent)
            {
                thisPlayer = player;
            }
            else
            {
                Opponent = player;
            }
        }

        controller = new AIPlayerController();
        thisPlayer.Controller = controller;

        Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();

        var gamePlayState = FindObjectOfType<GamePlayState>();
        gamePlayState.OnTeamScored += (team) =>
        {
            if(team == thisPlayer.PlayerTeam)
            {
                AddReward(1);
            }
            else
            {
                AddReward(-0.2f);
            }

            EndEpisode();
        };

        this.MaxStep = 100;
        if (Academy.Instance.IsCommunicatorOn)
        {
            this.MaxStep = 5000;
            ForceReset = gamePlayState.ResetState;
        }
    }

    Action ForceReset = () => { };

    public override void OnEpisodeBegin()
    {
        ForceReset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // flip observations if on the red team
        var teamFlipperVec = Vector2.one;
        if(thisPlayer.PlayerTeam == Player.Team.Red)
        {
            teamFlipperVec.x = -1;
        }

        //first add info about ourselves
        sensor.AddObservation(Vector2.Scale(thisPlayer.transform.position, teamFlipperVec));
        sensor.AddObservation(Vector2.Scale(thisPlayer.GetComponent<Rigidbody2D>().velocity, teamFlipperVec));
        sensor.AddObservation((int)thisPlayer.PlayerFacing * teamFlipperVec.x);
        sensor.AddObservation(thisPlayer.Attacking);

        // then add info about opponent
        sensor.AddObservation(Vector2.Scale(Opponent.transform.position, teamFlipperVec));
        sensor.AddObservation(Vector2.Scale(Opponent.GetComponent<Rigidbody2D>().velocity, teamFlipperVec));

        // finally add info about ball
        sensor.AddObservation(Vector2.Scale(Ball.position, teamFlipperVec));
        sensor.AddObservation(Vector2.Scale(Ball.velocity, teamFlipperVec));
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // movement action
        this.controller.MovementAction = Mathf.FloorToInt(vectorAction[0]);
        if (this.controller.MovementAction == 2)
        {
            this.controller.MovementAction = -1;
        }

        if (thisPlayer.PlayerTeam == Player.Team.Red)
        {
            this.controller.MovementAction *= -1;
        }


        // button actions
        this.controller.JumpAction = false;
        this.controller.AttackAction = false;

        switch (Mathf.FloorToInt(vectorAction[1]))
        {
            case 1:
                this.controller.JumpAction = true;
                break;
            case 2:
                this.controller.AttackAction = true;
                break;
        }

    }

    public override void Heuristic(float[] actionsOut)
    {
    }
}
