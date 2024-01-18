/* ******************************************************************************
 *
 * HerculesPlugin.cs - Hercules Native Interface (Distribution)
 *
 * Created by Yongbin Jeong (canelia@com2us.com)
 * © Com2uS Platform Corp.
 *
 * ******************************************************************************/
//#undef UNITY_EDITOR
#if USE_HERCULES
using AOT;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;

// Native로 연결된 경우에는 난독화를 하더라도 적용되지 않기 때문에 원래 함수 이름을 알 수 없도록 함
// H000 InitializeHercules
// H001 InitializeHerculesUnity (deprecated)
// H002 UninitializeHercules
// H003 HerculesAddVar
// H004 HerculesRemoveVar
// H005 HerculesSetVar
// H006 HerculesGetVar
// H007 HerculesIsEmulator (Android only)
// H008 HerculesIsRootedDevice (Android only)
// H009 HerculesIsJailbrokenDevice (iOS only)
// H00A HerculesFreeMem
// H00B HerculesSetUserId
// H00C HerculesTouchEvent
// H00D HerculesSubmitLog (Deprecated)
// H00E HerculesSetCallback (Deprecated)
// H00F HerculesIsOfficialBuild (Deprecated)
// H010 HerculesGetVersion
// H011 HerculesIsUnofficialBuild (iOS only)
// H012 HerculesGetFunnels (Android only, native)
// H013 HerculesIsRepackaged (Android only)
// H014 HerculesSetScreenSize
// H019 HerculesOpenAuth
// H01A HerculesSetGameData
// H01B HerculesGetCertHash
// H01C HerculesGetCertDesc
// H01D HerculesGetTeamId
// H01E HerculesSetPlayerId
// H01F HerculesGetApkSignedTime
// H020 HerculesAddString
// H021 HerculesGetString
// H022 HerculesPrefsGetInt
// H023 HerculesPrefsGetFloat
// H024 HerculesPrefsGetString
// H025 HerculesPrefsSetInt
// H026 HerculesPrefsSetFloat
// H027 HerculesPrefsSetString
// H028 HerculesPrefsHasKey
// H029 HerculesPrefsDeleteKey
// H02A HerculesPrefsDeleteAll
// H02B HerculesPrefsSave
// H02C HerculesGetUniqueInstanceId
// H02D HerculesAppGuardCallback
// H02E HerculesSetDeviceId

public static class HerculesPlugin
{
	public delegate void MessageCallback(int code, string message);
	public delegate void VoidCallback();

	public const uint NONE = 0;
	public const uint REPORT_ANDROID_ID = 1;

	public const int AUTH_NORMAL = 0;
	public const int AUTH_SAVE_ID = 1;
	public const int AUTH_SAVE_PW = 2;
	public const int AUTH_AUTO_LOGIN = 3;

#if UNITY_STANDALONE_WIN // Windows
	static HerculesPlugin() { InitializeHercules("", ""); }
#else
	static HerculesPlugin() { InitializeHercules(""); }
#endif

#if !UNITY_EDITOR && UNITY_ANDROID // Android devices
	[DllImport ("Hercules")] private static extern uint H000(string a, MessageCallback b, uint c, IntPtr d, IntPtr e);
	[DllImport ("Hercules")] private static extern void H002();
	[DllImport ("Hercules")] public static extern unsafe ulong H003(void *a, uint b);
	[DllImport ("Hercules")] public static extern void H004(ulong a);
	[DllImport ("Hercules")] public static extern unsafe void H005(ulong a, void *b);
	[DllImport ("Hercules")] public static extern unsafe int H006(ulong a, void *b);
	[DllImport ("Hercules")] private static extern uint H007();
	[DllImport ("Hercules")] private static extern uint H008();
	[DllImport ("Hercules")] public static extern void H00A(IntPtr a);
	[DllImport ("Hercules")] private static extern void H00B(string a);
	[DllImport ("Hercules")] private static extern void H00C(int a, int b, int c);
	//[DllImport ("Hercules")] private static extern void H00D();
	[DllImport ("Hercules")] private static extern uint H010();
	[DllImport ("Hercules")] private static extern IntPtr H012();
	[DllImport ("Hercules")] private static extern uint H013();
	[DllImport ("Hercules")] private static extern void H014(int a, int b);
	[DllImport ("Hercules")] private static extern int H019(int a, VoidCallback b);
	[DllImport ("Hercules")] private static extern uint H01A(string a);
	[DllImport ("Hercules")] private static extern IntPtr H01B();
	[DllImport ("Hercules")] private static extern IntPtr H01C();
	[DllImport ("Hercules")] private static extern void H01E(string a);
	[DllImport ("Hercules")] private static extern uint H01F();
	[DllImport ("Hercules")] public static extern ulong H020(string a);
	[DllImport ("Hercules")] public static extern IntPtr H021(ulong a);
	[DllImport ("Hercules")] public static extern int H022(string a, int b);
	[DllImport ("Hercules")] public static extern float H023(string a, float b);
	[DllImport ("Hercules")] public static extern IntPtr H024(string a, string b);
	[DllImport ("Hercules")] public static extern void H025(string a, int b);
	[DllImport ("Hercules")] public static extern void H026(string a, float b);
	[DllImport ("Hercules")] public static extern void H027(string a, string b);
	[DllImport ("Hercules")] public static extern int H028(string a);
	[DllImport ("Hercules")] public static extern void H029(string a);
	[DllImport ("Hercules")] public static extern void H02A();
	[DllImport ("Hercules")] public static extern void H02B();
	[DllImport ("Hercules")] private static extern IntPtr H02C();
	[DllImport ("Hercules")] private static extern void H02E(string a);

	public static uint InitializeHercules(string appName, MessageCallback cb = null, uint flags = NONE)
	{
		// Debug.Log("[Hercules] InitializeHercules - App:" + appName + ", Thread: " + Thread.CurrentThread.ManagedThreadId);
		defaultCallback = cb;
		try {
			AndroidJNI.AttachCurrentThread(); // Thread.CurrentThread.ManagedThreadId != 1
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			IntPtr activity = jo == null ? new IntPtr(0) : jo.GetRawObject();
			return H000(appName, InvokeMessageCallback, flags, new IntPtr(0), activity);
		} catch (Exception e) {
			Debug.Log($"[Hercules] Exception: {e}");
			return H000(appName, InvokeMessageCallback, flags, new IntPtr(0), new IntPtr(0));
		}
	}
	public static void UninitializeHercules() { H002(); }
	public static uint IsJailbrokenDevice() { return 0; } // iOS
	public static uint IsUnofficialBuild() { return 0; } // iOS
	public static uint IsEmulator() { return H007(); }
	public static uint IsRootedDevice() { return H008(); }
	public static uint IsRepackaged() { return H013(); }
	public static string GetFunnels() { return Marshal.PtrToStringAnsi(H012()); }
	public static string GetCertHash() { return Marshal.PtrToStringAnsi(H01B()); }
	public static string GetCertDesc() { return Marshal.PtrToStringAnsi(H01C()); }
	public static string GetTeamId() { return ""; } // iOS
	public static uint GetVersion() { return H010(); }
	public static uint GetApkSignedTime() { return H01F(); }
	public static void SetUserId(string userId) { H00B(userId); }
	public static void SetPlayerId(string playerId) { H01E(playerId); }
	public static void SetDeviceId(string deviceId) { H02E(deviceId); }
	public static uint SetGameData(string jsonString) { return H01A(jsonString); }
	public static void SetScreenSize(int width, int height) { H014(width, height); }
	public static void TouchEvent(int x, int y, int flag) { H00C(x, y, flag); }
	public static int OpenAuth(int loginType, VoidCallback cb = null)
	{
		authCallback = cb;
		return H019(loginType, InvokeAuthCallback);
	}
	public static string GetUniqueInstanceId() { return Marshal.PtrToStringAnsi(H02C()); }

#elif !UNITY_EDITOR && (UNITY_IOS || UNITY_IPHONE) // iOS Devices

	[DllImport ("__Internal")] private static extern uint H000(string a, MessageCallback b, uint c);
	[DllImport ("__Internal")] private static extern void H002();
	[DllImport ("__Internal")] public static extern unsafe ulong H003(void *a, uint b);
	[DllImport ("__Internal")] public static extern void H004(ulong a);
	[DllImport ("__Internal")] public static extern unsafe void H005(ulong a, void *b);
	[DllImport ("__Internal")] public static extern unsafe int H006(ulong a, void *b);
	[DllImport ("__Internal")] private static extern uint H009();
	[DllImport ("__Internal")] private static extern void H00B(string a);
	[DllImport ("__Internal")] private static extern void H00C(int a, int b, int c);
	//[DllImport ("__Internal")] private static extern void H00D();
	[DllImport ("__Internal")] public static extern void H00A(IntPtr a);
	[DllImport ("__Internal")] private static extern uint H010();
	[DllImport ("__Internal")] private static extern uint H011();
	[DllImport ("__Internal")] private static extern void H014(int a, int b);
	[DllImport ("__Internal")] private static extern int H019(int a, VoidCallback b, int c);
	[DllImport ("__Internal")] private static extern uint H01A(string a);
	[DllImport ("__Internal")] private static extern IntPtr H01C();
	[DllImport ("__Internal")] private static extern IntPtr H01D();
	[DllImport ("__Internal")] private static extern void H01E(string a);
	[DllImport ("__Internal")] public static extern ulong H020(string a);
	[DllImport ("__Internal")] public static extern IntPtr H021(ulong a);
	[DllImport ("__Internal")] public static extern int H022(string a, int b);
	[DllImport ("__Internal")] public static extern float H023(string a, float b);
	[DllImport ("__Internal")] public static extern IntPtr H024(string a, string b);
	[DllImport ("__Internal")] public static extern void H025(string a, int b);
	[DllImport ("__Internal")] public static extern void H026(string a, float b);
	[DllImport ("__Internal")] public static extern void H027(string a, string b);
	[DllImport ("__Internal")] public static extern int H028(string a);
	[DllImport ("__Internal")] public static extern void H029(string a);
	[DllImport ("__Internal")] public static extern void H02A();
	[DllImport ("__Internal")] public static extern void H02B();
	[DllImport ("__Internal")] private static extern IntPtr H02C();
	[DllImport ("__Internal")] private static extern void H02E(string a);

	public static uint InitializeHercules(string appName, MessageCallback cb = null, uint flags = NONE)
	{
		defaultCallback = cb;
		return H000(appName, InvokeMessageCallback, flags);
	}
	public static void UninitializeHercules() { H002(); }
	public static uint IsJailbrokenDevice() { return H009(); }
	public static uint IsUnofficialBuild() { return H011(); }
	public static uint IsEmulator() { return 0; } // Android
	public static uint IsRootedDevice() { return 0; } // Android
	public static uint IsRepackaged() { return 0; } // Android
	public static string GetFunnels() { return ""; } // Android
	public static string GetCertHash() { return ""; } // Android
	public static string GetCertDesc() { return Marshal.PtrToStringAnsi(H01C()); }
	public static string GetTeamId() { return Marshal.PtrToStringAnsi(H01D()); }
	public static uint GetVersion() { return H010(); }
	public static uint GetApkSignedTime() { return 0; } // Android
	public static void SetUserId(string userId) { H00B(userId); }
	public static void SetPlayerId(string playerId) { H01E(playerId); }
	public static void SetDeviceId(string deviceId) { H02E(deviceId); }
	public static uint SetGameData(string jsonString) { return H01A(jsonString); }
	public static void SetScreenSize(int width, int height) { H014(width, height); }
	public static void TouchEvent(int x, int y, int flag) { H00C(x, y, flag); }
	public static int OpenAuth(int loginType, VoidCallback cb = null)
	{
		authCallback = cb;
		return H019(loginType, InvokeAuthCallback, 1); // 0:SetView, 1:Presentation
	}
	public static string GetUniqueInstanceId() { return Marshal.PtrToStringAnsi(H02C()); }

#elif !UNITY_EDITOR && UNITY_STANDALONE_WIN // Windows
	[DllImport("Hercules")] private static extern uint H000(string a, MessageCallback b, uint c, string d, string e);
	[DllImport("Hercules")] private static extern void H002();
	[DllImport("Hercules")] public static extern unsafe ulong H003(void *a, uint b);
	[DllImport("Hercules")] public static extern void H004(ulong a);
	[DllImport("Hercules")] public static extern unsafe void H005(ulong a, void *b);
	[DllImport("Hercules")] public static extern unsafe int H006(ulong a, void *b);
	[DllImport("Hercules")] public static extern void H00A(IntPtr a);
	[DllImport("Hercules")] private static extern void H00B(string a);
	[DllImport("Hercules")] private static extern void H00C(int a, int b, int c);
	[DllImport("Hercules")] private static extern uint H010();
	[DllImport("Hercules")] private static extern void H014(int a, int b);
	[DllImport("Hercules")] private static extern int H019(int a, VoidCallback b);
	[DllImport("Hercules")] private static extern uint H01A(string a);
	[DllImport("Hercules")] private static extern void H01E(string a);
	[DllImport("Hercules")] public static extern ulong H020(string a);
	[DllImport("Hercules")] public static extern IntPtr H021(ulong a);
	[DllImport("Hercules")] public static extern int H022(string a, int b);
	[DllImport("Hercules")] public static extern float H023(string a, float b);
	[DllImport("Hercules")] public static extern IntPtr H024(string a, string b);
	[DllImport("Hercules")] public static extern void H025(string a, int b);
	[DllImport("Hercules")] public static extern void H026(string a, float b);
	[DllImport("Hercules")] public static extern void H027(string a, string b);
	[DllImport("Hercules")] public static extern int H028(string a);
	[DllImport("Hercules")] public static extern void H029(string a);
	[DllImport("Hercules")] public static extern void H02A();
	[DllImport("Hercules")] public static extern void H02B();
	[DllImport("Hercules")] private static extern IntPtr H02C();
	[DllImport("Hercules")] private static extern void H02E(string a);

	public static uint InitializeHercules(string appName, string appId, MessageCallback cb = null, uint flags = NONE)
	{
		defaultCallback = cb;
		return H000(appName, InvokeMessageCallback, flags, appId, Application.version);
	}
	public static void UninitializeHercules() { H002(); }
	public static uint IsEmulator() { return 0; } // Android
	public static uint IsRootedDevice() { return 0; } // Android
	public static uint IsJailbrokenDevice() { return 0; } // iOS
	public static uint IsUnofficialBuild() { return 0; } // iOS
	public static uint IsRepackaged() { return 0; } // Android
	public static string GetFunnels() { return ""; } // Android
	public static string GetCertHash() { return ""; } // Android
	public static string GetCertDesc() { return ""; } // Android, iOS
	public static string GetTeamId() { return ""; } // iOS
	public static uint GetVersion() { return H010(); }
	public static uint GetApkSignedTime() { return 0; } // Android
	public static void SetUserId(string userId) { H00B(userId); }
	public static void SetPlayerId(string playerId) { H01E(playerId); }
	public static void SetDeviceId(string deviceId) { H02E(deviceId); }
	public static uint SetGameData(string jsonString) { return H01A(jsonString); }
	public static void SetScreenSize(int width, int height) { H014(width, height); }
	public static void TouchEvent(int x, int y, int flag) { H00C(x, y, flag); }
	public static int OpenAuth(int loginType, VoidCallback cb = null)
	{
		authCallback = cb;
		return H019(loginType, InvokeAuthCallback);
	}
	public static string GetUniqueInstanceId() { return Marshal.PtrToStringAnsi(H02C()); }
	public static void AppGuardCallback(int code, string data) { }

#else // Unity Editor
	#if UNITY_STANDALONE_WIN // Windows
		public static uint InitializeHercules(string appName, string appId, MessageCallback cb = null, uint flags = NONE) { return 0; }
	#else
		public static uint InitializeHercules(string appName, MessageCallback cb = null, uint flags = NONE) { return 0; }
	#endif

	public static void UninitializeHercules() { }
	public static uint IsJailbrokenDevice() { return 0; }
	public static uint IsUnofficialBuild() { return 0; }
	public static uint IsEmulator() { return 0; }
	public static uint IsRootedDevice() { return 0; }
	public static uint IsRepackaged() { return 0; }
	public static string GetFunnels() { return "Unity Editor"; }
	public static string GetCertHash() { return ""; }
	public static string GetCertDesc() { return ""; }
	public static string GetTeamId() { return ""; }
	public static uint GetVersion() { return 0; }
	public static uint GetApkSignedTime() { return 0; }
	public static void SetUserId(string userId) { }
	public static void SetPlayerId(string playerId) { }
	public static void SetDeviceId(string deviceId) { }
	public static uint SetGameData(string jsonString) { return 0; }
	public static void SetScreenSize(int width, int height) { }
	public static void TouchEvent(int x, int y, int flag) { }
	public static int OpenAuth(int loginType, VoidCallback cb = null) { if (cb != null) cb(); return 0; }
	public static string GetUniqueInstanceId() { return "0000-0000-0000"; }
	public static void AppGuardCallback(int code, string data) { }
#endif

#if !UNITY_EDITOR
	private static MessageCallback defaultCallback = null;
	private static VoidCallback authCallback = null;

	[MonoPInvokeCallback(typeof(MessageCallback))]
	private static void InvokeMessageCallback(int code, string message)
	{
		if (defaultCallback != null)
			defaultCallback(code, message);
	}

	[MonoPInvokeCallback(typeof(VoidCallback))]
	private static void InvokeAuthCallback()
	{
		if (authCallback != null)
			authCallback();
	}
#endif
}


#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_WIN)
public class HerculesVarBase
{
	private ulong _seq = 0;
	public unsafe HerculesVarBase()
	{
		ulong v = 0;
		_seq = HerculesPlugin.H003(&v, 8);
	}
	~HerculesVarBase()
	{
		if (_seq != 0)
			HerculesPlugin.H004(_seq);
	}

	public unsafe void SetDouble(double fp)
	{
		HerculesPlugin.H005(_seq, &fp);
	}
	public unsafe void SetSigned(long sn)
	{
		HerculesPlugin.H005(_seq, &sn);
	}
	public unsafe void SetUnsigned(ulong un)
	{
		HerculesPlugin.H005(_seq, &un);
	}

	public unsafe double GetDouble()
	{
		double var = 0;
		HerculesPlugin.H006(_seq, &var);
		return var;
	}
	public unsafe long GetSigned()
	{
		long var = 0;
		HerculesPlugin.H006(_seq, &var);
		return var;
	}
	public unsafe ulong GetUnsigned()
	{
		ulong var = 0;
		HerculesPlugin.H006(_seq, &var);
		return var;
	}
}

public class HerculesString
{
	private ulong _seq = 0;
	public HerculesString(string str = "")
	{
		// #1
		// IntPtr unmanaged = (IntPtr)Marshal.StringToHGlobalAnsi(managedString);
		// Marshal.FreeHGlobal(unmanaged);

		// #2
		// byte[] bytes = Encoding.Default.GetBytes(str);
		// _length = bytes.length;
		// IntPtr unmanaged = Marshal.AllocHGlobal(_length);
		// Marshal.Copy(bytes, 0, unmanaged, _length);
		// _seq = HerculesPlugin.H003(unmanaged, _length);
		// Marshal.FreeHGlobal(unmanaged);

		// #3
		_seq = HerculesPlugin.H020(str);
	}

	~HerculesString()
	{
		if (_seq != 0)
			HerculesPlugin.H004(_seq);
	}

	public override string ToString()
	{
		IntPtr ptr = HerculesPlugin.H021(_seq);
		if (ptr == IntPtr.Zero)
			return "";
		string str = Marshal.PtrToStringAnsi(ptr);
		HerculesPlugin.H00A(ptr);
		return str;
	}

	public static implicit operator HerculesString(string str)
	{
		return new HerculesString(str);
	}
}

public class HerculesPrefs
{
	public static int GetInt(string key, int defaultValue = 0)
	{
		return HerculesPlugin.H022(key, defaultValue);
	}
	public static float GetFloat(string key, float defaultValue = 0.0f)
	{
		return HerculesPlugin.H023(key, defaultValue);
	}
	public static string GetString(string key, string defaultValue = "")
	{
		IntPtr ptr = HerculesPlugin.H024(key, defaultValue);
		if (ptr != IntPtr.Zero) {
			defaultValue = Marshal.PtrToStringAnsi(ptr);
			HerculesPlugin.H00A(ptr);
		}
		return defaultValue;
	}

	public static void SetInt(string key, int value)
	{
		HerculesPlugin.H025(key, value);
	}
	public static void SetFloat(string key, float value)
	{
		HerculesPlugin.H026(key, value);
	}
	public static void SetString(string key, string value)
	{
		HerculesPlugin.H027(key, value);
	}

	public static bool HasKey(string key)
	{
		return HerculesPlugin.H028(key) != 0 ? true : false;
	}
	public static void DeleteKey(string key)
	{
		HerculesPlugin.H029(key);
	}
	public static void DeleteAll()
	{
		HerculesPlugin.H02A();
	}
	public static void Save()
	{
		HerculesPlugin.H02B();
	}
}

#else // Unity Editor, other platform
public class HerculesVarBase
{
	double _fp;
	long _sn;
	ulong _un;

	public HerculesVarBase()
	{
		_fp = 0;
		_sn = 0;
		_un = 0;
	}

	public void SetDouble(double fp)
	{
		_fp = fp;
	}
	public void SetSigned(long sn)
	{
		_sn = sn;
	}
	public void SetUnsigned(ulong un)
	{
		_un = un;
	}
	public double GetDouble()
	{
		return _fp;
	}
	public long GetSigned()
	{
		return _sn;
	}
	public ulong GetUnsigned()
	{
		return _un;
	}
}

public class HerculesString
{
	private string _str;
	public HerculesString(string str = "")
	{
		_str = str;
	}
	public override string ToString()
	{
		return _str;
	}
	public static implicit operator HerculesString(string str)
	{
		return new HerculesString(str);
	}
}

// Unity 2017 => error CS0509: `HerculesPrefs': cannot derive from sealed type `UnityEngine.PlayerPrefs'
// public class HerculesPrefs : PlayerPrefs { }

public class HerculesPrefs
{
	public static int GetInt(string key, int defaultValue = 0)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}
	public static float GetFloat(string key, float defaultValue = 0.0f)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}
	public static string GetString(string key, string defaultValue = "")
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}
	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}
	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}
	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}
	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
	public static void Save()
	{
		PlayerPrefs.Save();
	}
}

#endif

public class HerculesVar<T>
{
	//private Type _type;
	protected int _type; // 0:long, 1:ulong, 2:double, 3:bool, 4:enum
	protected HerculesVarBase _var = new HerculesVarBase();

	public HerculesVar(T value = default(T))
	{
		// int size = Marshal.SizeOf(value);
		if (value is float || value is double)
		{
			_type = 2;
		}
		else if (value is bool)
		{
			_type = 3;
		}
		else if (typeof(T).IsEnum)
		{
			_type = 4;
		}
		else if (Convert.ToBoolean(typeof(T).GetField("MinValue").GetValue(null)))
		{
			_type = 0; // -?? to bool => true (signed)
		}
		else
		{
			_type = 1; // 0 to bool => false (unsigned)
		}
		Set(value);
	}

	public void Set(T value)
	{
		if (_type == 0) // signed
			_var.SetSigned(Convert.ToInt64(value));
		else if (_type == 1) // unsigned
			_var.SetUnsigned(Convert.ToUInt64(value));
		else if (_type == 2) // float, double
			_var.SetDouble(Convert.ToDouble(value));
		else // signed, bool, enum, ...
			_var.SetSigned(Convert.ToInt64(value));
	}

	public override string ToString()
	{
		if (_type == 0)
			return _var.GetSigned().ToString();
		if (_type == 1)
			return _var.GetUnsigned().ToString();
		if (_type == 2)
			return _var.GetDouble().ToString();
		if (_type == 3)
			return _var.GetSigned() != 0 ? "true" : "false";
		if (_type == 4)
			return Enum.ToObject(typeof(T), _var.GetSigned()).ToString();
		return _var.GetSigned().ToString();
	}

	// Assign value to HerculesVar
	public static implicit operator HerculesVar<T>(T value)
	{
		return new HerculesVar<T>(value);
	}

	// 1 byte types
	public static implicit operator bool(HerculesVar<T> value)
	{
		return value._var.GetSigned() != 0 ? true : false;
	}
	public static implicit operator char(HerculesVar<T> value)
	{
		return (char)(long)value;
	}
	public static implicit operator sbyte(HerculesVar<T> value)
	{
		return (sbyte)(long)value;
	}
	public static implicit operator byte(HerculesVar<T> value)
	{
		return (byte)(ulong)value;
	}

	// 2 bytes integer
	public static implicit operator short(HerculesVar<T> value)
	{
		return (short)(long)value;
	}
	public static implicit operator ushort(HerculesVar<T> value)
	{
		return (ushort)(ulong)value;
	}

	// 4bytes integer
	public static implicit operator int(HerculesVar<T> value)
	{
		return (int)(long)value;
	}
	public static implicit operator uint(HerculesVar<T> value)
	{
		return (uint)(ulong)value;
	}

	// 8bytes integer
	public static implicit operator long(HerculesVar<T> value)
	{
		if (value._type == 0 || value._type == 4)
			return (long)value._var.GetSigned();
		if (value._type == 1)
			return (long)value._var.GetUnsigned();
		if (value._type == 2)
			return (long)value._var.GetDouble();
		if (value._type == 3)
			return (long)(value._var.GetSigned() == 0 ? 0.0 : 1.0);
		return (long)value._var.GetSigned();
	}
	public static implicit operator ulong(HerculesVar<T> value)
	{
		if (value._type == 0 || value._type == 4)
			return (ulong)value._var.GetSigned();
		if (value._type == 1)
			return (ulong)value._var.GetUnsigned();
		if (value._type == 2)
			return (ulong)value._var.GetDouble();
		if (value._type == 3)
			return (ulong)(value._var.GetSigned() == 0 ? 0.0 : 1.0);
		return (ulong)value._var.GetUnsigned();
	}

	// floating point
	public static implicit operator float(HerculesVar<T> value)
	{
		return (float)(double)value;
	}
	public static implicit operator double(HerculesVar<T> value)
	{
		if (value._type == 0 || value._type == 4)
			return (double)value._var.GetSigned();
		if (value._type == 1)
			return (double)value._var.GetUnsigned();
		if (value._type == 2)
			return (double)value._var.GetDouble();
		if (value._type == 3)
			return (double)(value._var.GetSigned() == 0 ? 0.0 : 1.0);
		return (double)value._var.GetDouble();
	}
}

public class HerculesEnum<T> : HerculesVar<T>
{
	public HerculesEnum(T value) : base(value) { }

	public static implicit operator HerculesEnum<T>(T value)
	{
		return new HerculesEnum<T>(value);
	}

	public static implicit operator T(HerculesEnum<T> value)
	{
		return (T)Enum.ToObject(typeof(T), value._var.GetSigned());
	}
}
#endif