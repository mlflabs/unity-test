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

    [SerializeField] private Image panelImage;

    [SerializeField] private Button selectButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    private InventoryPanelUi inventoryPanel;

    //[SerializeField] private Transform transform;

     
    private void Start() {

      inventoryPanel = this.GetComponentInParent<InventoryPanelUi>();
      selectButton = GetChildComponentByName<Button>("SelectButton");
      useButton = GetChildComponentByName<Button>("UseButton");
      dropButton = GetChildComponentByName<Button>("DropButton");
      Debug.Log(selectButton);
      Debug.Log(useButton);
      Debug.Log(dropButton);
      inventoryPanel.onSelectedItemChanged += onInventorySelectionChanged;
      
      selectButton.onClick.AddListener(onSelectButtonClicked);
      useButton.onClick.AddListener(onUseButtonClicked);
      dropButton.onClick.AddListener(onDropButtonClicked);

    }    


    public void setItem(InventorySlot s) {
      this.transform.localScale = new Vector3(1,1,1);
      //Debug.Log("----------------Setting item::::: " + s.item.name);
      slot = s;
      text.text = slot.item.name + " (" + slot.amount + ")";
      //Debug.Log(slot.item.icon);
      image.sprite = slot.item.icon;
      onInventorySelectionChanged(inventoryPanel.selectedItem);
    }

    public void hide() {
      //Debug.Log("Hide");
      this.transform.localScale = new Vector3(0,0,0);
    }

    private void onInventorySelectionChanged(PanelItemUi item) {
      Debug.Log("on inventory select changed");
      if(item == this) {
        panelImage.color = Color.gray;
        useButton.transform.localScale = new Vector3(1,1,1);
        dropButton.transform.localScale = new Vector3(1,1,1);
      }
      else {
        panelImage.color = Color.white;
        useButton.transform.localScale = new Vector3(0,0,0);
        dropButton.transform.localScale = new Vector3(0,0,0);
      }
    }

    private void onSelectButtonClicked() {
      Debug.Log("on select btn clicked");
      inventoryPanel.setSelectedItem(this);
    }

    private void onUseButtonClicked() {
      Debug.Log("on use btn clicked");

      slot.item.onItemUse();
      
    }

    private void onDropButtonClicked() {
      Debug.Log("on drop btn clicked");
     
    }







    private T GetChildComponentByName<T>(string name) where T : Component {
        foreach (T component in GetComponentsInChildren<T>(true)) {
            if (component.gameObject.name == name) {
                return component;
            }
        }
        return null;
    }

      
  }
}
