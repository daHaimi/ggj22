using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public Vector2 roomPosition;
    public float transitionDuration = .5f;
    public List<GameObject> playerCharacters;

    private bool inProgress = false;

    private GameObject trigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // when the GameObjects collider arrange for this GameObject to travel to the left of the screen
    public void SwitchRoom(Vector2 direction, GameObject go)
    {
        if (inProgress)
        {
            return;
        }

        inProgress = true;
        trigger = go;
        trigger.GetComponent<Rigidbody2D>().simulated = false;
        trigger.GetComponent<PlayerControls>().otherPlayer.GetComponent<Rigidbody2D>().simulated = false;
        
        GameControls controls = Camera.main.GetComponent<GameControls>();
        Vector2 newRoomPos = controls.curRoom + direction;
        direction *= new Vector2(1, -1);
        
        Vector2 target = Camera.main.transform.position + new Vector3(controls.roomSize.x * direction.x, controls.roomSize.y * direction.y, 0);
        StartCoroutine(LerpPosition(target));

        Vector3 newPos = trigger.transform.position;
        newPos.x += direction.x * 2.4f;
        newPos.y += direction.y * 2.4f;
        trigger.GetComponent<Rigidbody2D>().transform.position = newPos;
        trigger.GetComponent<PlayerControls>().otherPlayer.transform.position = newPos;
        // Update Room
        Camera.main.GetComponent<GameControls>().SetCurRoom(newRoomPos);
        controls.rooms[controls.curRoom].GetComponent<RoomBehaviour>().EnterRoom();
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition)
    {
        float time = 0;
        Vector2 startPosition = Camera.main.transform.position;

        while (time < transitionDuration)
        {
            Vector3 target = Vector2.Lerp(startPosition, targetPosition, time / transitionDuration);
            Camera.main.transform.position = target + Vector3.back;
            time += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = targetPosition + (Vector3.back * 10);
        inProgress = false;
        trigger.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        trigger.gameObject.GetComponent<PlayerControls>().otherPlayer.GetComponent<Rigidbody2D>().simulated = true;
    }
}
