package md57dbc339f34da99f8b27b7a3eee02e78b;


public class FruitDetailScreen
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("TaskyAndroid.Screens.FruitDetailScreen, TaskyAndroid, Version=1.0.5730.17490, Culture=neutral, PublicKeyToken=null", FruitDetailScreen.class, __md_methods);
	}


	public FruitDetailScreen () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FruitDetailScreen.class)
			mono.android.TypeManager.Activate ("TaskyAndroid.Screens.FruitDetailScreen, TaskyAndroid, Version=1.0.5730.17490, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
