using System;
using System.Runtime.InteropServices;

namespace CodeFiend.WbmDrivers.external
{
	public static class WBM5000
	{
		private const string DllFile = "./external/WBM_5000.dll";

		[DllImport(DllFile)]
		public static extern IntPtr CommOpen(string Port);

		[DllImport(DllFile)]
		public static extern int WBM5000_ResetMove(IntPtr ComHandle, byte Reset_Type);

		[DllImport(DllFile)]
		public static extern int WBM5000_CardMove(IntPtr ComHandle, byte Move_Type, out byte ERR_Code);

		[DllImport(DllFile)]
		public static extern int WBM5000_CardIN(IntPtr ComHandle, byte CardIn_Type1, byte CardIn_Type2, out byte ERR_Code);

		[DllImport(DllFile, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern int WBM5000_MagCardReadData(IntPtr ComHandle, byte Mode, byte Track_Type, out byte RLEN, [MarshalAs(UnmanagedType.LPArray, SizeConst = 100)]  byte[] TrackData);

		[DllImport(DllFile, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern int WBM5000_SensorState(IntPtr ComHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] byte[] SenState, out byte ERR_Code);

		[DllImport(DllFile)]
		public static extern int CommClose(IntPtr ComHandle);
	}
}