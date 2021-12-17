using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KFKPlayerAgent : Agent
{
    Player thisPlayer;

    AIPlayerController controller;

    Player opponent;

    Rigidbody2D ball;

    public override void Initialize()
    {
        thisPlayer = this.transform.parent.GetComponent<Player>();

        controller = new AIPlayerController();
        thisPlayer.Controller = controller;

        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();

        foreach (var player in FindObjectsOfType<Player>())
        {
            if (player != thisPlayer)
            {
                opponent = player;
            }
        }

        var gameplayState = FindObjectOfType<GamePlayState>();
        gameplayState.OnTeamScored += (team) =>
        {
            if (team == thisPlayer.PlayerTeam)
            {
                AddReward(1);
            }
            else
            {
                AddReward(-0.2f);
            }

            EndEpisode();
        };

        if (Academy.Instance.IsCommunicatorOn)
        {
            ForceReset = gameplayState.ResetState;
        }
    }

    Action ForceReset = () => { };

    public override void OnEpisodeBegin()
    {
        ForceReset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // flip observations depending on the team 
        var teamFlipperVec = Vector2.one;
        if(thisPlayer.PlayerTeam == Player.Team.Red)
        {
            teamFlipperVec.x = -1;
        }

        // first add information about ourself
        sensor.AddObservation(Vector2.Scale(thisPlayer.transform.position, teamFlipperVec));
        sensor.AddObservation(Vector2.Scale(thisPlayer.Body.velocity, teamFlipperVec));
        sensor.AddObservation((int)thisPlayer.PlayerFacing * teamFlipperVec.x);
        sensor.AddObservation(thisPlayer.Attacking);

        // then add info about the opponent
        sensor.AddObservation(Vector2.Scale(opponent.transform.position, teamFlipperVec));
        sensor.AddObservation(Vector2.Scale(opponent.Body.velocity, teamFlipperVec));

        // Finally add info about the ball
        sensor.AddObservation(Vector2.Scale(ball.position, teamFlipperVec));
        sensor.AddObservation(Vector2.Scale(ball.velocity, teamFlipperVec));

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // movement actions
        var movement = Mathf.FloorToInt(vectorAction[0]);
        if(movement == 2)
        {
            movement = -1;
        }
        controller.MovementAction = movement;

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
}
