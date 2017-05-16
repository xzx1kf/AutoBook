using System;
using Gtk;

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

	protected void OnBtnSearchClicked (object sender, EventArgs e)
	{
		// See which checkboxes are checked. 
		if (cbTue.Active) {
			textview1.Buffer.Text = "Tuesday";
		}
	}
}
