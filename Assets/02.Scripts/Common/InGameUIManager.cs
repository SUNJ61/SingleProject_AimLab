using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
     public static InGameUIManager instance;

     private GameObject RandomShootGameScoreBoard;
     private TMP_Text RandomShootGameScoreTxt;
     private TMP_Text RandomShootGameLevelTxt;

     private void Awake()
     {
          if (instance == null)
               instance = this;
          else if (instance != this)
               Destroy(gameObject);

          RandomShootGameScoreBoard = GameObject.Find("RandomShootGameScoreBoard");
          RandomShootGameScoreTxt = RandomShootGameScoreBoard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
          RandomShootGameLevelTxt = RandomShootGameScoreBoard.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
     }

     public void RandomShootGameButtonHit(RaycastHit hit)
     {
          var button = hit.transform.gameObject.GetComponent<Button>();
          if (button != null)
          {
               button.onClick.Invoke(); //버튼에 등록된 이벤트 발생.
          }
     }

     public void RandomShootGameScore(int score)
     {
          RandomShootGameScoreTxt.text = $"Score : {score}";
     }

     public void RandomShootGameLevelText(int level)
     {
          switch(level)
          {
               case 0:
                    RandomShootGameLevelTxt.text = "Level : Normal";
               break;

               case 1:
                    RandomShootGameLevelTxt.text = "Level : Hard";
               break;
          }
     }
}
