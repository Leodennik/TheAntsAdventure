 using UnityEngine;

 public class GameMenu : MonoBehaviour
 {
     public void Show()
     {
         gameObject.SetActive(true);
     }
     
     public void Hide()
     {
         gameObject.SetActive(false);
     }
 }
