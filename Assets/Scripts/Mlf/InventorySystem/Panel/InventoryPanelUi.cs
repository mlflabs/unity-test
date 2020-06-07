using System.Collections;
using System.Collections.Generic;
using Mlf.Gm;
using Mlf.InventorySystem.Base;
using Mlf.InventorySystem.Items;
using Mlf.Sm.BasicStateMachine;
using UnityEngine;


namespace Mlf.InventorySystem.Panel {
  public class InventoryPanelUi : MonoBehaviour
  {
      
      [SerializeField] private Vector3 onScreenPosition = new Vector3(-63, 260, 0);
      [SerializeField] private Vector3 offScreenPosition = new Vector3(100, 260, 0);

      [SerializeField] private RectTransform rectTransform;
      public InventoryData inventory = new InventoryData();
      public GameObject inventorySlotPrefab;

      public List<PanelItemUi> slots = new List<PanelItemUi>();
      public int numberOfSlots = 6;
      
      void Start()
      {

        /*for(int i = 0; i < 16; i ++) {
          GameObject go = Instantiate(inventorySlotPrefab);
          go.transform.SetParent(this.transform);
          go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, -1);
          InventoryPanelItemUi ui = go.GetComponent<InventoryPanelItemUi>();
          Debug.Log(ui);
          ui.hide();
          slots.Add(ui);          
        }
        */
        rectTransform = GetComponent<RectTransform>();

        PanelItemUi[] allChildren = GetComponentsInChildren<PanelItemUi>();


        foreach(PanelItemUi ui in allChildren) {
          slots.Add(ui);
        }

        //Debug.Log(")))))))))))))))))))))))))))))) " + allChildren.Length);
        //Debug.Log(allChildren);




        //Debug.Log(GameManager.instance);
        GameManager.instance.onSelectedSmChanged += onSmChanged;

        onSmChanged(null);
        

          
      }

      private void onSmChanged(BasicSm sm) {

        //changing, so unsubscribe
        if(inventory != null)
          inventory.onInventoryUpdate -= onInventoryChanged;

        if(sm == null) {
          rectTransform.anchoredPosition = offScreenPosition;
          return;
        }
        
        this.inventory = sm.inventory;
        this.inventory.onInventoryUpdate += onInventoryChanged;
        onInventoryChanged();
        

        rectTransform.anchoredPosition = onScreenPosition;
        Debug.Log("======================== " + inventory.items.Count);
      
      }

      private void onInventoryChanged() {
        Debug.Log("Inventory update.............................");
        
        for(int i = 0; i < slots.Count; i++) {
          if(i < inventory.items.Count) {
            slots[i].setItem(inventory.items[i]);
          }
          else {
            slots[i].hide();
          }
        }

      }

  }
}
