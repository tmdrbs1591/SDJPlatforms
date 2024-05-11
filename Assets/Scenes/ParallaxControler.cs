using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxControler : MonoBehaviour
{
    public bool Camera_Move;
    public float Camera_MoveSpeed = 1.5f;
    [Header("Layer Setting")]
    public float[] Layer_Speed = new float[7];
    public GameObject[] Layer_Objects = new GameObject[7];

    private Transform _camera;
    private float[] startPos = new float[7];
    private float boundSizeX;
    private float sizeX;

    private Transform playerTransform;

    public float CameraFollowSpeed = 5.0f;

    void Start()
    {
        _camera = Camera.main.transform;
        sizeX = Layer_Objects[0].transform.localScale.x;
        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        for (int i = 0; i < 5; i++)
        {
            startPos[i] = _camera.position.x;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // �÷��̾��� �����ӿ� ���� ī�޶� �̵�
        if (Camera_Move)
        {
            _camera.position += Vector3.right * Time.fixedDeltaTime * Camera_MoveSpeed;
        }

        if (playerTransform != null)
        {
            // �÷��̾��� ��ġ�� ���� ī�޶� ���󰡵���
            Vector3 targetPosition = new Vector3(playerTransform.position.x, _camera.position.y, _camera.position.z);
            _camera.position = Vector3.Lerp(_camera.position, targetPosition, Time.deltaTime * CameraFollowSpeed);
        }

        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            float temp = (_camera.position.x * (1 - Layer_Speed[i]));
            float distance = _camera.position.x * Layer_Speed[i];
            Layer_Objects[i].transform.position = new Vector2(startPos[i] + distance, _camera.position.y + 1f); // Y ��ǥ�� �����Ͽ� ī�޶��� �ణ ���� ��ġ�ϵ��� ��
            if (temp > startPos[i] + boundSizeX * sizeX)
            {
                startPos[i] += boundSizeX * sizeX;
            }
            else if (temp < startPos[i] - boundSizeX * sizeX)
            {
                startPos[i] -= boundSizeX * sizeX;
            }
        }
    }
}
