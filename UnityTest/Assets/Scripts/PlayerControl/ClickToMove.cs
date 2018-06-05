using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {
	private NavMeshAgent _agent;
	private PlayerAnimation _animation;
	
	private SceneNode _targetNode;
	private bool _isMoving;
	
        
	void Start() {
		_agent = GetComponent<NavMeshAgent>();
		_animation = GetComponent<PlayerAnimation>();
		_isMoving = false;
	}
        
	void Update()
	{
		// stop moving
		if (_isMoving && _agent.remainingDistance < 0.0005f) 
		{
			_isMoving = false;
			_animation.CharacterIdle();
			if (_targetNode != null)
				_animation.FlipX = _targetNode.NodePosition.position.x > gameObject.transform.position.x;
		}
		
		// handle input
		HandleInput();
	}
	
	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				if (hit.collider.gameObject.CompareTag("StaticNode") == true)
				{
					_targetNode = hit.collider.gameObject.GetComponent<SceneNode>();
					MoveToMouse(_targetNode.ArriveLocation);
				}
				else
				{
					_targetNode = null;
					MoveToMouse(hit.point);
				}

				_animation.CharacterMove();
				_animation.FlipX = hit.point.x > gameObject.transform.position.x;
			}
		}
	}

	private void MoveToMouse(Vector3 destination)
	{
		Vector3 aimVector = destination - gameObject.transform.position;
		_agent.velocity = aimVector.normalized * _agent.speed;
		_agent.destination = destination;
		_isMoving = true;
	}
}