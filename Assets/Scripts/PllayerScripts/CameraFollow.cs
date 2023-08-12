using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	Transform target;

	[SerializeField]  float lookSpeed = 0.2f;
	[SerializeField]  float followSpeed = 0.2f;

	[SerializeField]
	Vector3 camOffset;

    private void Start()
    {
		target = FindObjectOfType<PlayerStateManager>().transform;
    }
    void LateUpdate()
	{
		LookAtTarget();
		FollowTarget();
	}

	void LookAtTarget()
	{
		Vector3 lookDir = target.position - this.transform.position;
		Quaternion rot = Quaternion.LookRotation(lookDir, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
	}

	void FollowTarget()
	{
		Vector3 targetPos = target.position + target.forward * camOffset.z + target.right * camOffset.x + target.up * camOffset.y;
		this.transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
	}
}

