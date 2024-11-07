using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
     public static InGameUIManager instance; 

     private void Awake()
     {
          if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
     }

     public void RandomShootGameUI(RaycastHit hit)
     {
          var button = hit.transform.gameObject.GetComponent<Button>();
          if (button != null)
          {
               button.onClick.Invoke(); //버튼에 등록된 이벤트 발생.
          }
     }
}
