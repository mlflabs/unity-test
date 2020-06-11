
using Mlf.Gm.InputTypes;
using Mlf.Sm.BasicStateMachine;
using UnityEngine;

namespace Mlf.Gm.Behaviours
{
  public class GetPointerInput : MonoBehaviour {


    public BasicUserDragInput dragTarget;


    public void Start () {
        
    }

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
          Debug.Log("Left Mouse Button");
          Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

          RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
          if (hit.collider != null) {
            Debug.Log("============================");
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.collider.gameObject.tag);
            if(hit.collider.gameObject.tag == "UserInput") {
              Debug.Log("Player Selected");
              GameManager.instance.selectedObject = hit.collider.gameObject;
            }

            if(dragTarget == null) {
              BasicUserDragInput bdi = hit.collider.gameObject.GetComponent<BasicUserDragInput>();
              if(bdi != null) {
                dragTarget = bdi;
                dragTarget.startDrag();
              }
            }
            //hit.collider.attachedRigidbody.AddForce(Vector2.up);
          }

          if(dragTarget != null) {
            dragTarget.setNewDragPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
          }
      }

      if(Input.GetMouseButtonUp(0)) {
        if(dragTarget == null) return;

        dragTarget.stopDrag();
        dragTarget = null;
      }

      if(Input.GetMouseButtonDown(1)) {
        Debug.Log("Right Mouse Button");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null) {
            // we hit an object, see what object it is, and react
            if(GameManager.instance.selectedObject != null) {
            }
          }
          else {
            if(GameManager.instance.selectedObject != null) {
              //can we move it
              BasicSm sm = GameManager.instance.selectedObject.GetComponent<BasicSm>();
              if(sm != null) {
                Debug.Log("Setting Destination:::: " + mousePos2D);
                sm.motionData.destination = mousePos2D;
              }
            }
          }
      }
    }
  } 
}



