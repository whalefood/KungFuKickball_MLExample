using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : MonoBehaviour
{
    public enum PlayerControlType
    {
        None,
        AI,
        Keyboard
    }


    int redTeamScore;

    int blueTeamScore;

    [SerializeField]
    Player BlueTeamPlayer;

    [SerializeField]
    Player RedTeamPlayer;

    [SerializeField]
    Rigidbody2D Ball;

    public Action ResetState { get; private set; }

    public Action<Player.Team> OnTeamScored = (t) => { };

    public PlayerControlType BluePlayerControllerType;

    public PlayerControlType RedPlayerControllerType;

    public void RedTeamScored()
    {
        redTeamScore++;
        OnTeamScored(Player.Team.Red);
        ResetState();
    }

    public void BlueTeamScored()
    {
        blueTeamScore++;
        OnTeamScored(Player.Team.Blue);
        ResetState();
    }

    void SetPlayerControllerByType(Player player, PlayerControlType controlType)
    {
        if(controlType == PlayerControlType.Keyboard)
        {
            player.Controller = new KeyboardPlayerController();
        }
        else if(controlType == PlayerControlType.AI)
        {
            var AI = player.GetComponentInChildren<KFKPlayerAgent>(true);
            AI.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {

        var ballStartPos = this.Ball.position;
        ResetState = () =>
        {
            this.Ball.position = ballStartPos;
            this.Ball.velocity = new Vector2(0, 10); // serve ball
            this.Ball.angularVelocity = 0;
            BlueTeamPlayer.ResetPosition();
            RedTeamPlayer.ResetPosition();
        };

        SetPlayerControllerByType(BlueTeamPlayer, BluePlayerControllerType);
        SetPlayerControllerByType(RedTeamPlayer, RedPlayerControllerType);

    }


    // Update is called once per frame
    void Update()
    {
        

    }
}
