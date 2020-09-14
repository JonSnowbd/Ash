namespace Coga
{
    public class CogaValue
    {
        public CogaValueMode ValueType { get; internal set; }

        public float Value;

        /// <summary>
        /// Create a defined pixel Value.
        /// </summary>
        /// <param name="val"></param>
        public CogaValue(int val)
        {
            ValueType = CogaValueMode.Pixel;
            Value = val;
        }

		public CogaValue(float val, CogaValueMode mode)
		{
			ValueType = mode;
			Value = val;
		}

		public void Set(float val, CogaValueMode mode)
		{
			ValueType = mode;
			Value = val;
		}

		public void Clear()
		{
			ValueType = CogaValueMode.Undefined;
		}
    }
}
