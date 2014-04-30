using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content.PM;
using System.IO;
using System.Collections.Generic;

namespace FileBrowserApp{
	[Activity (Label = "FileBrowserApp", MainLauncher = true)]
	public class MainActivity : Activity {
		String currentDir = "/";

		GestureDetector gestureDetector;
		GestureListener gestureListener;


		//Things to do when initialized
		protected override void OnCreate (Bundle bundle){

			base.OnCreate (bundle);
			// Set view from the main layout resource
			SetContentView (Resource.Layout.Main);
			gestureListener = new GestureListener();
			gestureListener.SwipeLeftEvent += GestureLeft;
			gestureDetector = new GestureDetector (this, gestureListener);
			TableLayout table = FindViewById<TableLayout>(Resource.Id.table);

			//Button home = FindViewById<Button> (Resource.Id.home);

			table.SetBackgroundColor (Android.Graphics.Color.LightCyan);

			FrameLayout frame = FindViewById<FrameLayout> (Resource.Id.frameLayout);
			//when frame layout (Home dir button) is clicked, it goes home!
			frame.Click += delegate {
				homeDir();
			};
			//list/re-list all the directories
			listDirectory ();


		}

		//the way to list all directories, with folders first and files second.
		private void listDirectory(){
			TableLayout table = FindViewById<TableLayout>(Resource.Id.table);
			table.RemoveAllViews ();


			//trying to list files and folders
			try{
				//Adding back button
				var directories = Directory.EnumerateDirectories(currentDir);
				if (currentDir != "/") {
					TableRow row = new TableRow (this);
					TextView text = new TextView (this);
					ImageView image = new ImageView(this);
					image.SetImageResource(Resource.Drawable.folder2);
					text.Text = "Back";
					//listener for clicking on back button
					row.Click += (object sender, EventArgs e) => {
						backDir();
					};
					//add everything to the table view
					row.AddView(image);
					row.AddView (text);
					table.AddView (row);
				}

				//listing folders
				foreach (var directory in directories) {
					TableRow row = new TableRow (this);
					TextView text = new TextView (this);
					ImageView image = new ImageView(this);
					image.SetImageResource(Resource.Drawable.folder2);

					text.Text = directory;
					text.SetTextColor(Android.Graphics.Color.Black);
					text.SetPadding(0,50,0,50);
					text.SetTextSize(Android.Util.ComplexUnitType.Px,50);
						row.SetBackgroundColor(Color.LightCyan);
					//listening for a click on a folder
					row.Click += (object sender, EventArgs e) => {
						currentDir = directory;
						listDirectory();
					};
					//add everything to the table layout
					row.AddView(image);
					row.AddView (text);
					table.AddView (row);
				}

				//listing files
				var files = Directory.GetFiles (currentDir);
				foreach (var file in files) {
					TableRow row = new TableRow (this);
					TextView text = new TextView (this);
					ImageView image = new ImageView(this);
					image.SetImageResource(Resource.Drawable.File);

					text.Text = file;
					text.SetTextColor(Android.Graphics.Color.Black);
					text.SetPadding(0,50,0,50);
					text.SetTextSize(Android.Util.ComplexUnitType.Px,50);
					//listen for a click on a file, open a file chooser with a list of possible programs.				
					row.Click += (object sender, EventArgs e) =>  {
						Intent intent = new Intent(Intent.ActionSend);
						String title = "How would you like to open this file?";
						//						IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
						//create an intent to show chooser
						Intent chooser = Intent.CreateChooser(intent,title);
						//if (availableActivities.Count > 0){
							StartActivity(chooser);
						//}
					};
					//add more things to the table layout!
					row.AddView(image);
					row.AddView (text);
					table.AddView (row);
				}
				//if you don't have permissions, go back a folder and give an error message!
			}catch(Exception){
				backDir ();
				AlertDialog alertMessage = new AlertDialog.Builder(this).Create();
				alertMessage.SetTitle("Permissions Error");
				alertMessage.SetMessage("You do not have permissions to access this folder");
				alertMessage.Show();
			}


		}
		//go back a folder by substringing the current path and refresh the page
		private void backDir(){
			if (currentDir.LastIndexOf ("/") != 0) {
				currentDir = currentDir.Substring (0, currentDir.LastIndexOf ("/"));
			} else {
				currentDir = "/";
			}
			listDirectory ();
		}

		//go straight to the root directory
		public void homeDir(){
			currentDir = "/";
			listDirectory ();
		}

		//when the back button of the phone is pressed, either go back a folder, or exit the program.
		public override void OnBackPressed(){
			if (currentDir == "/") {
				Finish ();
			}
			backDir ();
		}

		//when someone swipes left to right, go back a directory
		void GestureLeft(MotionEvent first, MotionEvent second)
		{
			backDir ();
			//int position = 0;//listView.PointToPosition ((int)second.GetX (), (int)second.GetY ());
			//Toast.MakeText (this, formatMotionEventAttributes(first, second) , ToastLength.Long).Show ();
			Toast.MakeText (this, "You swipe left on the " + 0, ToastLength.Long).Show ();
		}
	}
}


