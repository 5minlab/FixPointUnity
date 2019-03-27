using System;
using UniRx;
using FixMath;

namespace FixMath.UniRx
{
	[Serializable]
	public class F32ReactiveProperty : ReactiveProperty<F32>
	{
		public F32ReactiveProperty()
		{
		}

		public F32ReactiveProperty(F32 initialValue)
			: base(initialValue)
		{
		}
	}
}
