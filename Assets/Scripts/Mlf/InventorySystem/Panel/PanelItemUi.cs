using System.Collections;
using System.Collections.Generic;
using Mlf.Gm;
using Mlf.InventorySystem.Base;
using Mlf.Sm.BasicStateMachine.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Mlf.InventorySystem.Panel {
  public class PanelItemUi : MonoBehaviour
  {
      
    public InventorySlot slot;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    //[SerializeField] private Transform transform;

     
    private void Awake() {
      //transform = GetComponent<Transform>();
      //Debug.Log("99999999999999 Transform:::: " + transform);
    }

    public void setItem(InventorySlot s) {
      this.transform.localScale = new Vector3(1,1,1);
      //Debug.Log("----------------Setting item::::: " + s.item.name);
      slot = s;
      text.text = slot.item.name + " (" + slot.amount + ")";
      //Debug.Log(slot.item.icon);
      image.sprite = slot.item.icon;
    }

    public void hide() {
      //Debug.Log("Hide");
      this.transform.localScale = new Vector3(0,0,0);
    }

      
  }
}
