using System;

namespace Coga
{
	public class ComputedValue
	{
		public bool Resolved;
		public float Value;

		public ComputedValue()
		{
			Resolved = false;
			Value = 0f;
		}

		public void Complete(float f)
		{
			Value = f;
			Resolved = true;
		}

		public void Clear()
		{
			Value = 0f;
			Resolved = false;
		}
	}
}
