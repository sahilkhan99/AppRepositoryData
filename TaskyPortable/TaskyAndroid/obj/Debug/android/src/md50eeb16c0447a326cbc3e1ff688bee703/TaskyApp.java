package md50eeb16c0447a326cbc3e1ff688bee703;


public class TaskyApp
	extends mono.android.app.Application
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:()V:GetOnCreateHandler\n" +
			"";
	}


	public TaskyApp () throws java.lang.Throwable
	{
		super ();
	}

	public void onCreate ()
	{
		mono.android.Runtime.register ("TaskyAndroid.TaskyApp, TaskyAndroid, Version=1.0.5729.25135, Culture=neutral, PublicKeyToken=null", TaskyApp.class, __md_methods);
		n_onCreate ();
	}

	private native void n_onCreate ();

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
