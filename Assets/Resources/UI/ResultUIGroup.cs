using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIGroup : UIBehaviour<ResultUIGroup>
{
    [SerializeField] private BowlUIGage[] bowlGage;
    [SerializeField] private Image winnerImage;
    [SerializeField] private Sprite[] winnerSprite;

    private int winner;


    // Start is called before the first frame update
    void Start()
    {
        SetWinner(TeamState.Kinoko);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetWinner(TeamState team)
    {
        winner = (int)team;

        bowlGage[winner].SetWinner();

        SetWinnerImage();
    }


    public void SetWinnerImage()
    {
        switch(winner)
        {
            case (int)TeamState.Kinoko:
                winnerImage.sprite = winnerSprite[0];
                break;

            case (int)TeamState.Takenoko:
                winnerImage.sprite = winnerSprite[1];
                break;
        }
    }
}
