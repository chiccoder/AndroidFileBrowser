using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.IO;
using System.Collections.Generic;
namespace FileBrowserApp {
	[Activity (Label = "FileBrowserApp", MainLauncher = true)]
	public class MainActivity : Activity{
		String currentDir = "/";
		protected override void OnCreate (Bundle bundle){

			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			TableLayout table = FindViewById<TableLayout>(Resource.Id.table);
			table.SetBackgroundColor (Android.Graphics.Color.BlanchedAlmond);
			listDirectory ();


		}

		private void listDirectory(){
			TableLayout table = FindViewById<TableLayout>(Resource.Id.table);
			table.RemoveAllViews ();


			//trying to list files and folders
			try{

				//Adding back availabliblity
				var directories = Directory.EnumerateDirectories(currentDir);
				if (currentDir != "/") {
					TableRow row = new TableRow (this);
					TextView text = new TextView (this);
					ImageView image = new ImageView(this);
					image.SetImageResource(Resource.Drawable.Folder);
					text.Text = "..";

					row.Click += (object sender, EventArgs e) => {
						backDir();
					};

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
					text.SetTextColor(Android.Graphics.Color.Green);

					row.AddView(image);
					row.AddView (text);
					table.AddView (row);
				}

				//listing folders
				bool color = true;
				foreach (var directory in directories) {
					TableRow row = new TableRow (this);
					TextView text = new TextView (this);
					ImageView image = new ImageView(this);
					image.SetImageResource(Resource.Drawable.Folder);

					text.Text = directory;
					text.SetTextColor(Android.Graphics.Color.White);
					if(color){
						row.SetBackgroundColor(Color.Aqua);
						color=false;
					}else{
						row.SetBackgroundColor(Color.Coral);
						color=true;
					}
					//text.SetHeight(14);

					row.Click += (object sender, EventArgs e) => {
						currentDir = directory;
						listDirectory();
					};

					row.AddView(image);
					row.AddView (text);
					table.AddView (row);
				}

			}catch(Exception){
				backDir ();

				AlertDialog alertMessage = new AlertDialog.Builder(this).Create();
				alertMessage.SetTitle("Permissions Error");
				alertMessage.SetMessage("You do not have permissions to access this folder");
				alertMessage.Show();
			}
		}

		private void backDir(){
			if (currentDir.LastIndexOf ("/") != 0) {
				currentDir = currentDir.Substring (0, currentDir.LastIndexOf ("/"));
			} else {
				currentDir = "/";
			}
			listDirectory ();
		}

		public override void OnBackPressed(){
			if (currentDir == "/") {
				Finish ();
			}
			backDir ();
		}
	}

}


