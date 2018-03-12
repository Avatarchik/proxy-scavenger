using System;
using com.ootii.Collections;
using com.ootii.Messages;
using com.ootii;


public class HazardMessage : Message {

	public HazardLevel HazardRating = HazardLevel.Low;
	public HazardType Hazard = HazardType.Heat;
	public int HazardIndex = 1;

	/// <summary>
	/// Clear the message content so it can be reused
	/// </summary>
	public override void Clear()
	{
		base.Clear();
		HazardRating = HazardLevel.None;
		Hazard = HazardType.None;
		HazardIndex = 0;
	}

	/// <summary>
	/// Release this instance.
	/// </summary>
	public override void Release()
	{
		// We should never release an instance unless we're
		// sure we're done with it. So clearing here is fine
		Clear();

		// Reset the sent flags. We do this so messages are flagged as 'completed'
		// and removed by default.
		IsSent = true;
		IsHandled = true;

		// Make it available to others.
		if (this is HazardMessage)
		{
			sPool.Release(this);
		}
	}

	// ******************************** OBJECT POOL ********************************

	/// <summary>
	/// Allows us to reuse objects without having to reallocate them over and over
	/// </summary>
	private static ObjectPool<HazardMessage> sPool = new ObjectPool<HazardMessage>(40, 10);

	/// <summary>
	/// Pulls an object from the pool.
	/// </summary>
	/// <returns></returns>
	public new static HazardMessage Allocate()
	{
		// Grab the next available object
		HazardMessage lInstance = sPool.Allocate();

		// Reset the sent flags. We do this so messages are flagged as 'completed'
		// by default.
		lInstance.IsSent = false;
		lInstance.IsHandled = false;

		// For this type, guarentee we have something
		// to hand back tot he caller
		if (lInstance == null) { lInstance = new HazardMessage(); }
		return lInstance;
	}

	/// <summary>
	/// Returns an element back to the pool.
	/// </summary>
	/// <param name="rEdge"></param>
	public static void Release(HazardMessage rInstance)
	{
		if (rInstance == null) { return; }
		rInstance.Clear();

		// Reset the sent flags. We do this so messages are flagged as 'completed'
		// and removed by default.
		rInstance.IsSent = true;
		rInstance.IsHandled = true;

		// Make it available to others.
		sPool.Release(rInstance);
	}

	/// <summary>
	/// Returns an element back to the pool.
	/// </summary>
	/// <param name="rEdge"></param>
	public new static void Release(IMessage rInstance)
	{
		if (rInstance == null) { return; }

		// We should never release an instance unless we're
		// sure we're done with it. So clearing here is fine
		rInstance.Clear();

		// Reset the sent flags. We do this so messages are flagged as 'completed'
		// and removed by default.
		rInstance.IsSent = true;
		rInstance.IsHandled = true;

		// Make it available to others.
		if (rInstance is HazardMessage)
		{
			sPool.Release((HazardMessage)rInstance);
		}
	}
}
