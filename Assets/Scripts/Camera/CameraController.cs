using UnityEngine;
 
public class CameraController : MonoBehaviour {
 
    [SerializeField] private float movementSpeed = 1.0f;
    private bool cameraLocked = true;

    void Update () {
        //movementSpeed = Mathf.Max(movementSpeed += Input.GetAxis("Mouse ScrollWheel"), 0.0f);
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            Cursor.visible = !Cursor.visible;
            this.cameraLocked = !cameraLocked;
        }
        if(!cameraLocked){
            transform.position += (
                transform.right * Input.GetAxis("Horizontal") + 
                transform.forward * Input.GetAxis("Vertical") + 
                transform.up * Input.GetAxis("Depth")) * movementSpeed;
            transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }
    }
}