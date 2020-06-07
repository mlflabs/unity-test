
using Mlf.Sm.BasicStateMachine;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.Gm.Behaviours
{
  public class GetPointerInput : MonoBehaviour {
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

            //hit.collider.attachedRigidbody.AddForce(Vector2.up);
          }
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



