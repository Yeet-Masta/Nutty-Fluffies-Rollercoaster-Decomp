using System;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00241
	{
		protected __SharingManager_Awake_0024callable0_002487_63__ _0024from;

		public _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00241(__SharingManager_Awake_0024callable0_002487_63__ from)
		{
			_0024from = from;
		}

		public void Invoke(object message)
		{
			_0024from();
		}

		public static Action<object> Adapt(__SharingManager_Awake_0024callable0_002487_63__ from)
		{
			return new _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00241(from).Invoke;
		}
	}
}
