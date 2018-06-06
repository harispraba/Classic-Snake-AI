using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    public float moveSpeed;
    [Tooltip("Delay in second between every movement")]
    public float delayPerMove;

    private Vector2 moveDirection;
    private Vector2 targetLocation;
    private bool isMoving;
    private bool isGameOver;

    class MoveDirection {
        Vector2 direction;

        public MoveDirection(Vector2 dir) {
            direction = dir;
        }

        public Vector2 getDirection() {
            return direction;
        }
    }
    
    // Use this for initialization
    void Start () {
        moveDirection = Vector2.up;

        StartCoroutine(Action());
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoving) {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetLocation, step);
            
            Vector2 vectorPos = new Vector2(transform.position.x, transform.position.y);

            //Debug.Log("Current pos: "+ transform.position + ". Target pos: " + targetLocation);
            if (vectorPos == targetLocation) {
                Debug.Log("Location reached!");
                isMoving = false;

                if (isGameOver) {
                    UIController.instance.ShowGameOverUI();
                }
            }
        }
	}

    IEnumerator Action() {
        while (!isGameOver) {
            yield return new WaitForSeconds(delayPerMove);
            while (isMoving) {
                yield return null;
            }

            List<Vector2> availableDirection = CheckSurrounding();

            if (availableDirection.Count == 1) {
                moveDirection = availableDirection[0];
            }
            else if(availableDirection.Count == 2) {
                int choice = Random.Range(0 , 2);
                moveDirection = availableDirection[choice];
            }
            else if(availableDirection.Count == 3) {
                int randomNum = Random.Range(1, 11);
                if (randomNum < 3)
                    moveDirection = -transform.right;
                else if (randomNum > 8)
                    moveDirection = transform.right;
                else
                    moveDirection = transform.up;
            }
            else {
                moveDirection = transform.up;
                isGameOver = true;
            }
            Debug.Log("Move direction: " + moveDirection);
            isMoving = true;

            //SetPlayerFacing();
            transform.up = moveDirection;

            targetLocation = new Vector2(transform.position.x + moveDirection.x,
                    transform.position.y + moveDirection.y);
            //Debug.Log("Start moving to " + targetLocation);
        }
    }

    List<Vector2> CheckSurrounding() {
        Debug.Log("Checking surrounding...");

        List<Vector2> moveDir = new List<Vector2>();

        Vector2 rayLeftOrigin, rayTopOrigin, rayRightOrigin;
        rayLeftOrigin = new Vector2(transform.position.x - .2f, transform.position.y);
        rayTopOrigin = new Vector2(transform.position.x, transform.position.y + .2f);
        rayRightOrigin = new Vector2(transform.position.x + .2f, transform.position.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(rayLeftOrigin, -transform.right, 1);
        RaycastHit2D hitTop = Physics2D.Raycast(rayTopOrigin, transform.up, 1);
        RaycastHit2D hitRight = Physics2D.Raycast(rayRightOrigin, transform.right, 1);

        if (!hitLeft.collider) {
            moveDir.Add(-transform.right);
            Debug.Log("No collider on the left!");
        }
        if (!hitTop.collider) {
            moveDir.Add(transform.up);
            Debug.Log("No collider on the front!");
        }
        if (!hitRight.collider) {
            moveDir.Add(transform.right);
            Debug.Log("No collider on the right!");
        }
        
        return moveDir;
    }

    void SetPlayerFacing()
    {
        Vector3 rotationAxis = new Vector3(0, 0, 1);
        if (moveDirection == new Vector2(-transform.right.x, -transform.right.y))
            transform.rotation = Quaternion.AngleAxis(90, rotationAxis);
        else if (moveDirection == new Vector2(transform.right.x, transform.right.y))
            transform.rotation = Quaternion.AngleAxis(-90, rotationAxis);
    }
}
