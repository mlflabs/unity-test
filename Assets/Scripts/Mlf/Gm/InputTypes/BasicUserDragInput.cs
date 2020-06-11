

using UnityEngine;

namespace Mlf.Gm.InputTypes {

  public abstract class BasicUserDragInput : MonoBehaviour {
    
      private bool isDragging;
      





    public void setNewDragPosition(Vector3 position) {

    }

    public void startDrag() {
      isDragging = true;
    }

    public void stopDrag() {
      isDragging = false;
    }

  }
}