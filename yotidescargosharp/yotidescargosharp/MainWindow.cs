// MainWindow.cs created with MonoDevelop
// User: manuelinux at 03:31 p 30/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using System.Diagnostics;
public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnBtnokClicked (object sender, System.EventArgs e)
	{
		if(txturl.Text==String.Empty||txtname.Text==String.Empty)
		{
			MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent,MessageType.Error, ButtonsType.Close, "Fields must not be empty");
		int result = md.Run ();
		md.Destroy();
		}
		else
		{
			if(!txturl.Text.Contains("http://"))
			{
				MessageDialog md = new MessageDialog(this,DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Please Introduce a valid URL");
				int result=md.Run();
				md.Destroy();
			}
			else
				downloadStart();
		}
		
	}
	public void downloadStart()
	{
		Process download = new Process();
		download.StartInfo.FileName="youtube-dl";
		download.StartInfo.Arguments="-o "+ txtpath.CurrentFolder+"/"+txtname.Text+".flv "+txturl.Text;
		MessageDialog md = new MessageDialog(this,DialogFlags.Modal, MessageType.Error, ButtonsType.Close, "I'll be back");
		int result=md.Run();
		md.Destroy();
		this.Hide();
		download.Start();
		while(!download.HasExited);
		this.Show();
		txturl.Text="";
		txtname.Text="";
		txturl.GrabFocus();
		
	}
}