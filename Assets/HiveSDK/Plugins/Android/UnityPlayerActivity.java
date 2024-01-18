package com.hive;

import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.pm.Signature;
import android.content.res.Configuration;
import android.os.Bundle;
import android.widget.Toast;

import java.io.ByteArrayInputStream;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;

import javax.security.auth.x500.X500Principal;

public class UnityPlayerActivity extends com.unity3d.player.UnityPlayerActivity
{


	@Override protected void attachBaseContext(Context newBase) {
		super.attachBaseContext(HiveActivity.attachBaseContext(newBase));
	}

	// Setup activity layout
	@Override protected void onCreate (Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);

		try {
			Signature raw = getPackageManager().getPackageInfo(getPackageName(), PackageManager.GET_SIGNATURES).signatures[0];

			CertificateFactory cf = CertificateFactory.getInstance("X.509");

			X509Certificate cert = (X509Certificate) cf.generateCertificate(new ByteArrayInputStream(raw.toByteArray()));

			boolean debug = cert.getSubjectX500Principal().equals(new X500Principal("CN=Android Debug,O=Android,C=US"));

			if (debug)
				Toast.makeText(this, "This signing is debug signing.", Toast.LENGTH_LONG).show();

		} catch (Exception e) {
			e.printStackTrace();
		}

		HiveActivity.onCreate(this, savedInstanceState);
	}

	// Start Unity
    @Override protected void onStart()
    {
        super.onStart();
        HiveActivity.onStart(this);
    }

    // Stop Unity
    @Override protected void onStop()
    {
        HiveActivity.onStop(this);
        super.onStop();
    }

	// Quit Unity
	@Override protected void onDestroy ()
	{
		HiveActivity.onDestroy(this);
		super.onDestroy();
	}

	// Pause Unity
	@Override protected void onPause()
	{
		HiveActivity.onPause(this);
		super.onPause();
	}

	// Resume Unity
	@Override protected void onResume()
	{
		super.onResume();
		HiveActivity.onResume(this);
	}

	// This ensures the layout will be correct.
	@Override public void onConfigurationChanged(Configuration newConfig)
	{
		HiveActivity.onConfigurationChanged(this, newConfig);
		super.onConfigurationChanged(newConfig);
	}

	// Notify Unity of the focus change.
	@Override public void onWindowFocusChanged(boolean hasFocus)
	{
		super.onWindowFocusChanged(hasFocus);
		HiveActivity.onWindowFocusChanged(this, hasFocus);
	}

	@Override
	protected void onNewIntent(Intent intent) 
	{
		super.onNewIntent(intent);
		HiveActivity.onNewIntent(this, intent);
	}

	@TargetApi(23) @Override
	public void onRequestPermissionsResult(int requestCode,
			String[] permissions, int[] grantResults) 
	{

		super.onRequestPermissionsResult(requestCode, permissions, grantResults);
		HiveActivity.onRequestPermissionsResult(this, requestCode, permissions, grantResults);
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) 
	{
		super.onActivityResult(requestCode, resultCode, data);
		HiveActivity.onActivityResult(this, requestCode, resultCode, data);
	}

	public void onSaveInstanceState(final Activity activity, final Bundle outState)
	{
		super.onSaveInstanceState(outState);
		HiveActivity.onSaveInstanceState(activity, outState);
	}

	@Override
	public void onMultiWindowModeChanged(boolean isInMultiWindowMode, android.content.res.Configuration newConfig) {
		super.onMultiWindowModeChanged(isInMultiWindowMode, newConfig);
		HiveActivity.onMultiWindowModeChanged(this, isInMultiWindowMode, newConfig);
	}

}
