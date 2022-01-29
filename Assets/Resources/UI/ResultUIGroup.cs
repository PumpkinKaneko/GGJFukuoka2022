using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIGroup : UIBehaviour<ResultUIGroup>
{
    [SerializeField] private BowlUIGage[] bowlGage;
    [SerializeField] private Image winnerImage;
    [SerializeField] private Sprite[] winnerSprite;
    [SerializeField] private GameObject[] characterArray;

    private int winner;
    private int loser;


    // Start is called before the first frame update
    void Start()
    {
        //UIManager.SetWinner(TeamState.Kinoko);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetWinner(TeamState team)
    {
        winner = (int)team;

        switch(winner)
        {
            case (int)TeamState.Kinoko:
                loser = (int)TeamState.Takenoko;
                break;

            case (int)TeamState.Takenoko:
                loser = (int)TeamState.Kinoko;
                break;
        }

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


    public void NockOut()
    {
        Debug.Log("Nock Out");

        characterArray[loser].GetComponent<Rigidbody>().AddForce(Vector3.forward * -50, ForceMode.Impulse);
        Destroy(characterArray[loser], 2f);
    }

}
