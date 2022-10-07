using System.Collections;
using UnityEngine;

public class GameRulesManager : MonoBehaviour
{
    [SerializeField]
    private PocketGrid pocketGrid;

    private void Start()
    {
        pocketGrid.PocketGridChanged.AddListener(MatchElements);
    }

    private void MatchElements()
    {
        //Vertical match check
        for (int x = 0; x < pocketGrid.GridSize.x; x++)
        {
            bool thereAreFreePockets = false;
            for (int y = 0; y < pocketGrid.GridSize.y; y++)
            {
                if (pocketGrid.pockets[x, y].MyGraySquare == null)
                {
                    thereAreFreePockets = true;
                    break;
                }
            }
            if (thereAreFreePockets) continue;

            for (int y = 0; y < pocketGrid.GridSize.y; y++)
                pocketGrid.pockets[x, y].RemoveGraySquare(y * 0.1f);

            StartCoroutine(Win());
            return;
        }

        //Horizontal match check
        for (int y = 0; y < pocketGrid.GridSize.y; y++)
        {
            bool thereAreFreePockets = false;
            for (int x = 0; x < pocketGrid.GridSize.x; x++)
            {
                if (pocketGrid.pockets[x, y].MyGraySquare == null)
                {
                    thereAreFreePockets = true;
                    break;
                }
            }
            if (thereAreFreePockets) continue;

            for (int x = 0; x < pocketGrid.GridSize.x; x++)
                pocketGrid.pockets[x, y].RemoveGraySquare(x * 0.1f);

            StartCoroutine(Win());
            return;
        }

        Lose();
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1f);
        UIController.Singleton.ShowResult("онаедю");
        Debug.Log("You are Win!");
    }

    private void Lose()
    {
        UIController.Singleton.ShowResult("ньхайю");
        Debug.Log("You are Lose!");
    }
}
