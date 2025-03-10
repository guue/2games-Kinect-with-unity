/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
//using Windows.Kinect;
using System.Collections;
using System;


public class RaiseHandListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	// singleton instance of the class
	private static RaiseHandListener instance = null;

	// whether the needed gesture has been detected or not
	private bool bRaiseLeftHand = false;
	private bool bRaiseRightHand = false;


	/// <summary>
	/// Gets the singleton RaiseHandListener instance.
	/// </summary>
	/// <value>The RaiseHandListener instance.</value>
	public static RaiseHandListener Instance
	{
		get
		{
			return instance;
		}
	}

	/// <summary>
	/// Determines whether the user has raised his left hand.
	/// </summary>
	/// <returns><c>true</c> if the user has raised his left hand; otherwise, <c>false</c>.</returns>
	public bool IsRaiseLeftHand()
	{
		if(bRaiseLeftHand)
		{
			bRaiseLeftHand = false;
			return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// Determines whether the user has raised his right hand.
	/// </summary>
	/// <returns><c>true</c> if the user has raised his right hand; otherwise, <c>false</c>.</returns>
	public bool IsRaiseRightHand()
	{
		if(bRaiseRightHand)
		{
			bRaiseRightHand = false;
			return true;
		}
		
		return false;
	}


	/// <summary>
	/// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserDetected(long userId, int userIndex)
	{
		KinectManager_fruit manager = KinectManager_fruit.Instance;

		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
	}

	/// <summary>
	/// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserLost(long userId, int userIndex)
	{
	}

	/// <summary>
	/// Invoked when a gesture is in progress.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="gesture">Gesture type</param>
	/// <param name="progress">Gesture progress [0..1]</param>
	/// <param name="joint">Joint type</param>
	/// <param name="screenPos">Normalized viewport position</param>
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
	}

	/// <summary>
	/// Invoked if a gesture is completed.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="gesture">Gesture type</param>
	/// <param name="joint">Joint type</param>
	/// <param name="screenPos">Normalized viewport position</param>
	public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint, Vector3 screenPos)
	{
		if(gesture == KinectGestures.Gestures.RaiseLeftHand)
			bRaiseLeftHand = true;
		else if(gesture == KinectGestures.Gestures.RaiseRightHand)
			bRaiseRightHand = true;
		
		return true;
	}

	/// <summary>
	/// Invoked if a gesture is cancelled.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="gesture">Gesture type</param>
	/// <param name="joint">Joint type</param>
	public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint)
	{
		if(gesture == KinectGestures.Gestures.RaiseLeftHand)
			bRaiseLeftHand = false;
		else if(gesture == KinectGestures.Gestures.RaiseRightHand)
			bRaiseRightHand = false;
		
		return true;
	}


	void Awake()
	{
		instance = this;
	}

}
