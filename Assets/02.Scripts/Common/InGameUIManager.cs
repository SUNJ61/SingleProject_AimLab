using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
     public static InGameUIManager instance;

     private GameObject InGameUICanvas;
     private GameObject RandomShootGameScoreBoard;
     private GameObject AIGunMatchScoreBoard;
     private GameObject ShopPanel;
     private GameObject OptionPanel;
     private TMP_Text RandomShootGameScoreTxt;
     private TMP_Text RandomShootGameLevelTxt;
     private TMP_Text AIGunMatchTimeTxt;
     private TMP_Text AIGunMatchLevelTxt;


     public bool areadyUI = false;
     public bool activeShopUI = false;
     private bool escapeUI = false;
     public bool EscapeUI
     {
          get { return escapeUI; }
          set
          {
               escapeUI = value;
               switch(areadyUI)
               {
                    case true:
                         ShopPanelActive(!EscapeUI);
                         OptionPanelActive(!EscapeUI);
                         areadyUI = false;
                    break;

                    case false:
                         OptionPanelActive(EscapeUI);
                    break;
               }
          }
     }
     private void Awake()
     {
          if (instance == null)
               instance = this;
          else if (instance != this)
               Destroy(gameObject);

          RandomShootGameScoreBoard = GameObject.Find("RandomShootGameScoreBoard");
          AIGunMatchScoreBoard = GameObject.Find("AIGunMatchScoreBoard");
          InGameUICanvas = GameObject.Find("InGameCanvas");
          ShopPanel = InGameUICanvas.transform.GetChild(1).gameObject;
          OptionPanel = InGameUICanvas.transform.GetChild(2).gameObject;

          RandomShootGameScoreTxt = RandomShootGameScoreBoard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
          RandomShootGameLevelTxt = RandomShootGameScoreBoard.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
          AIGunMatchTimeTxt = AIGunMatchScoreBoard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
          AIGunMatchLevelTxt = AIGunMatchScoreBoard.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
     }

     public void ShopPanelActive(bool active)
     {
          if(!activeShopUI && areadyUI) return;//다른 UI가 켜졌을 때 들어오지 않도록 하기 위해 사용.

          ShopPanel.SetActive(active);
          areadyUI = active;
          activeShopUI = active;
          GameManager.instance.CursorCtrl(active);
     }

     public void OptionPanelActive(bool active)
     {
          if(!EscapeUI && areadyUI) return;

          OptionPanel.SetActive(active);
          areadyUI = active;
          escapeUI = active;
          GameManager.instance.CursorCtrl(active);
     }

     public void RaycastButtonHit(RaycastHit hit)
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

     public void RandomShootGameLevelText(string level)
     {
          RandomShootGameLevelTxt.text = $"Level : {level}";
     }

     public void AIGunMatchTimeText(float time)
     {
          AIGunMatchTimeTxt.text = $"Time : {time.ToString("F1")}";
     }

     public void AIGunMatchLevelText(string level)
     {
          AIGunMatchLevelTxt.text = $"Level : {level}";
     }

     public void ExitGame()
     {
#if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;

#else
       Application.Quit();
#endif
     }
}
