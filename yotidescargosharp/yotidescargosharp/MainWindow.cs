// MainWindow.cs created with MonoDevelop
// User: manuelinux at 03:31 p 30/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using System.Diagnostics;
using System.Threading;

public partial class MainWindow: Gtk.Window
{	
	public String url;
	public String path;
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
			{
				url=txturl.Text;
				path=txtpath.CurrentFolder+"/"+txtname.Text;
				Thread down = new Thread(downloadStart);
				down.Start();
				
			}
			
		}
		
	}
	public void downloadStart()
	{
		Process download = new Process();
		download.StartInfo.FileName="youtube-dl";
		download.StartInfo.Arguments="-o "+path+".flv "+txturl.Text;
		lblstatus.Text="Downloading Video";
		System.Threading.Thread.Sleep(100);
		download.Start();
		while(!download.HasExited);
		if(chkavi.Active)
		{
			Thread avi = new Thread(convertToAvi);
			avi.Start();
		}
		else
		{
		lblstatus.Text="Ready";
		txturl.Text="";
		txtname.Text="";
		txturl.GrabFocus();
		}
	}
	public void convertToAvi()
	{
		Process convertavi = new Process();
		convertavi.StartInfo.FileName="ffmpeg";
		convertavi.StartInfo.Arguments="-i "+path+".flv "+path+".mpg";
		lblstatus.Text="Converting to AVI";
		convertavi.Start();
		while(!convertavi.HasExited);
		lblstatus.Text="Ready";
		txturl.Text="";
		txtname.Text="";
		txturl.GrabFocus();
	}
}