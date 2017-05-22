using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		//Build ();


		this.DeleteEvent += delete_event;
		this.BorderWidth = 10;

		// Court Labels
		Label lblCourt1 = new Label ("Court 1");
		lblCourt1.SetPadding (50, 0);
		Label lblCourt2 = new Label ("Court 2");
		lblCourt2.SetPadding (50, 0);
		Label lblCourt3 = new Label("Court 3");
		lblCourt3.SetPadding (50, 5);
		Label lblCourt4 = new Label("Court 4");
		lblCourt4.SetPadding (50, 0);
		Label lblCourt5 = new Label ("Court 5");
		lblCourt5.SetPadding (50, 0);

		/* Create a new button */
		Button btn = new Button();
		Button btn1 = new Button();
		btn.Name = "17/05/17 Court 1 17:40";
		btn1.Name = "17/05/17 Court 1 18:20";

		Gdk.Color col = new Gdk.Color();
		Gdk.Color.Parse("light green", ref col);

		btn.ModifyBg (StateType.Normal, col);
		btn1.ModifyBg (StateType.Normal, col);

		/* Connect the "clicked" signal of the button to our callback */
		btn.Clicked += callback;
		btn1.Clicked += callback;

		/* This calls our box creating function */
		Widget box = slot_label_box ("17th May", "17:40");
		Widget box1 = slot_label_box ("17th May", "18:20");

		/* Pack and show all our widgets */
		box.Show();
		box1.Show ();

		btn.Add(box);
		btn1.Add (box1);

		btn.Show();
		btn1.Show();

		Table table = new Table (3, 5, false);

		// Column Headers - Court Names
		table.Attach (lblCourt1, 0, 1, 0, 1);
		table.Attach (lblCourt2, 1, 2, 0, 1);
		table.Attach (lblCourt3, 2, 3, 0, 1);
		table.Attach (lblCourt4, 3, 4, 0, 1);
		table.Attach (lblCourt5, 4, 5, 0, 1);

		// Buttons - Booking Slots
		table.Attach (btn, 0, 1, 1, 2);
		table.Attach (btn1, 1, 2, 2, 3);

		this.Add(table);

		this.ShowAll();
	}

	/* Our usual callback function */
	static void callback( object obj, EventArgs args)
	{
			Console.WriteLine("Hello again - cool button was pressed " + ((Button)obj).Name);
	}

	/* another callback */
	static void delete_event (object obj, DeleteEventArgs args)
	{
		Application.Quit();
	}

	private static Widget slot_label_box( string date, string time )
	{
		VBox box = new VBox(false, 0);
		box.BorderWidth =  2;

		Label lblDate = new Label (date);
		Label lblTime = new Label (time);

		box.PackStart (lblDate, false, false, 3);
		box.PackEnd (lblTime, false, false, 3);

		lblDate.Show();
		lblTime.Show();

		return box;
	}

	private void CreateTreeView()
	{
		this.Title = "Squash Court Bookings";
		this.SetSizeRequest (400, 300);

		NodeView tree = new NodeView ();

		this.Add (tree);

		TreeViewColumn court1 = new TreeViewColumn ();
		court1.Title = "Court 1";

		TreeViewColumn court2 = new TreeViewColumn ();
		court2.Title = "Court 2";

		TreeViewColumn court3 = new TreeViewColumn ();
		court3.Title = "Court 3";

		TreeViewColumn court4 = new TreeViewColumn ();
		court4.Title = "Court 4";

		TreeViewColumn court5 = new TreeViewColumn ();
		court5.Title = "Court 5";

		tree.AppendColumn (court1);
		tree.AppendColumn (court2);
		tree.AppendColumn (court3);
		tree.AppendColumn (court4);
		tree.AppendColumn (court5);

		ListStore courtStore = new ListStore (typeof (string), typeof (string), typeof (string), typeof (string), typeof (string));

		tree.Model = courtStore;

		courtStore.AppendValues("17:40", "18:20", "17:40", "17:40", "17:40");
		courtStore.AppendValues("17:40", "18:20", "17:40", "17:40", "17:40");

		// Create the text cell that will display the artist name
		CellRendererText court1Cell = new CellRendererText ();

		// Add the cell to the column
		court1.PackStart (court1Cell, true);

		// Do the same for the song title column
		CellRendererText court2Cell = new CellRendererText ();
		court2Cell.Editable = true;
		court2.PackStart (court2Cell, true);

		// Tell the Cell Renderers which items in the model to display
		court1.AddAttribute (court1Cell, "text", 0);
		court2.AddAttribute (court2Cell, "text", 1);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
