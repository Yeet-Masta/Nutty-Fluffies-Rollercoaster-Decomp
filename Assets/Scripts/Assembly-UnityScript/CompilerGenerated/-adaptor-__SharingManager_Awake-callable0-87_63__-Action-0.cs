using System;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00240
	{
		protected __SharingManager_Awake_0024callable0_002487_63__ _0024from;

		public _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00240(__SharingManager_Awake_0024callable0_002487_63__ from)
		{
			_0024from = from;
		}

		public void Invoke(string name)
		{
			_0024from();
		}

		public static Action<string> Adapt(__SharingManager_Awake_0024callable0_002487_63__ from)
		{
			return new _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00240(from).Invoke;
		}
	}
}
