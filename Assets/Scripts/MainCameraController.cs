using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    
    public GameObject _player;
    public Vector2 margin = new Vector2(1, 1); // how much the player can move before the camera moves
    public Vector2 smoothing = new Vector2(3, 3); // how fast the camera moves
    public BoxCollider2D cameraBounds;
    public bool isFollowing = true;

    private Transform player;
    private Vector3 min, max;
    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        min = cameraBounds.bounds.min;
        max = cameraBounds.bounds.max;
        player = _player.GetComponent<Transform>();
        mainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        var x = transform.position.x;
        var y = transform.position.y;

        if (isFollowing)
        {
            /*
            if (Mathf.Abs(x - player.position.x) > margin.x)
                x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - player.position.y) > margin.y)
                y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
            */
            x = player.position.x;
            y = player.position.y;
        }

        var cameraHalfWidth = mainCamera.orthographicSize * ((float)Screen.width / Screen.height);

        x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, min.y + mainCamera.orthographicSize, max.y - mainCamera.orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);
	}
}
