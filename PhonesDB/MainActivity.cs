using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using PhonesDB.DB;
using Android.Widget;
using System.Linq;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidX.RecyclerView.Widget;
using Android.Content;
using System.Collections.Generic;

namespace PhonesDB
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private PhonesDBHelper _dBHelper;
        private bool _filterApplied = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            InitDB();
            DisplayAllPhones();
            DisplayAverageDiagonalSize();
            InitContacts();
            InitWarehouses();
        }

        private void InitDB()
        {
            _dBHelper = new PhonesDBHelper(this);
        }

        private void InitWarehouses()
        {
            var warehousesButton = FindViewById<Button>(Resource.Id.warehousesButton);
            warehousesButton.Click += (object sender, EventArgs e) =>
            {
                Intent nextActivity = new Intent(this, typeof(WarehousesActivity));
                StartActivity(nextActivity);
            };
        }

        private void InitContacts()
        {
            var contactsButton = FindViewById<Button>(Resource.Id.contactsButton);
            contactsButton.Click += (object sender, EventArgs e) =>
            {
                Intent nextActivity = new Intent(this, typeof(ContactsActivity));
                StartActivity(nextActivity);
            };
        }

        private void DisplayAllPhones()
        {
            var filterButton = FindViewById<Button>(Resource.Id.filterPhones);
            filterButton.Click += (object sender, EventArgs e) =>
            {
                filterButton.Text = _filterApplied ? "Показати усі моделі" : "Показати за фільтром";
                var phones = _filterApplied ? _dBHelper.GetPhonesByMinimalDiagonalSize("Motorola", 5) : _dBHelper.GetAllPhones();
                var list = FindViewById<ListView>(Resource.Id.mobile_list);
                var arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.activity_listview, Resource.Id.listtextview, phones.Select(p => p.Manufacturer + " " + p.Model + " " + p.DiagonalSize).ToArray());
                list.Adapter = arrayAdapter;
                _filterApplied = !_filterApplied;
            };
        }

        private void DisplayAverageDiagonalSize()
        {
            var diagonalSizeText = FindViewById<TextView>(Resource.Id.averageDiagonalSize);
            diagonalSizeText.Text = "Середня довжина діагоналі становить " + _dBHelper.GetAverageDiagonalSize();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
