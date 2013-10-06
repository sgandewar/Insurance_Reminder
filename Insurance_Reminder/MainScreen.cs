

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Ads;



namespace Insurance_Reminder
{
    [Activity(Label = "Inusrance Reminder", MainLauncher = true)]
    public class MainScreen : Activity
    {
        Button btnAddNew;
        TextView lblNoData;
        List<InsuranceReminderBO> listOfInsurance = new List<InsuranceReminderBO>();
        ListView listView;
        int selectedPosition;
        AdView adView;
         
        protected override void OnCreate(Bundle bundle)
        {

            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.Main);
                
                btnAddNew = FindViewById<Button>(Resource.Id.btnNew);
                lblNoData = FindViewById<TextView>(Resource.Id.lblNoData);
                listView = FindViewById<ListView>(Resource.Id.lstViewMain);

                RegisterForContextMenu(listView);

                btnAddNew.Click += btnAddNew_Click;
                listOfInsurance = GetAllReminders(this);
                if (listOfInsurance.Count.Equals(0))
                    lblNoData.Visibility = ViewStates.Visible;
                else
                {
                    lblNoData.Visibility = ViewStates.Gone;
                    listView.Adapter = new CustomListAdapter(this, listOfInsurance);
                    listView.ItemClick += listView_ItemClick;
                }

                // Create an ad.
                adView = FindViewById<AdView>(Resource.Id.ad);

                // Create an ad request.
                AdRequest adRequest = new AdRequest();
                adRequest.SetTesting(true);

                adRequest.AddTestDevice(AdRequest.TestEmulator);
                // If you're trying to show ads on device, use this.
                // The device ID to test will be shown on adb log.
                // adRequest.AddTestDevice (some_device_id);

                // Start loading the ad in the background.
                adView.LoadAd(adRequest);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message,ToastLength.Long);
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            selectedPosition = info.Position;
            menu.SetHeaderTitle(Resource.String.Options);
            menu.Add(0, 1, 1, Resource.String.UpdateDetails);
            menu.Add(0, 2, 2, Resource.String.Button_Delete_Text);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            try
            {
                switch (item.ItemId)
                {
                    case 1:                        
                        InsuranceReminderBO selectedInsurance = listOfInsurance[selectedPosition];
                        Toast.MakeText(this, String.Format("{0} selected !!!", selectedInsurance.ID.ToString()), ToastLength.Short).Show();
                        var intent = new Intent(this, typeof(UpdateDetails));
                        intent.PutExtra("UpdateObject", selectedInsurance);
                        StartActivity(intent);
                        return true;
                    case 2:
                        var builder = new AlertDialog.Builder(this);
                        builder.SetMessage(Resource.String.ShowDeleteConfirmationMessage);
                        builder.SetPositiveButton(Resource.String.DialogYesButton, DialogYesButtonClicked);
                        builder.SetNegativeButton(Resource.String.DialogNoButton, DialogNoButtonClicked);
                        builder.Show();
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        private void DialogYesButtonClicked(object sender, DialogClickEventArgs args)
        {
            try
            {
                var listView = sender as ListView;
                InsuranceReminderBO selectedInsurance = listOfInsurance[selectedPosition];
                InsuranceDataSource insuranceDataSource = new InsuranceDataSource(this);
                if (insuranceDataSource.DeleteInsuranceReminder(selectedInsurance))
                {
                    StartActivity(typeof(MainScreen));
                    this.Finish();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void DialogNoButtonClicked(object sender, DialogClickEventArgs args)
        {

        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            InsuranceReminderBO selectedInsurance = listOfInsurance[e.Position];
            Toast.MakeText(this, String.Format("{0} selected !!!", selectedInsurance.ID.ToString()), ToastLength.Short).Show();
            var intent = new Intent(this, typeof(ReminderDetails));
            intent.PutExtra("SelectedItem", selectedInsurance);
            StartActivity(intent);
        }

        void btnAddNew_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AddNewReminder));
        }

        private List<InsuranceReminderBO> GetAllReminders(Context context)
        {
            InsuranceDataSource insureDataSource = new InsuranceDataSource(context);
            List<InsuranceReminderBO> lst = insureDataSource.GetAllInsuranceReminders();
            return lst;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var MenuItemClose = menu.Add(0, 1, 1, Resource.String.Close);
            MenuItemClose.SetIcon(Resource.Drawable.close);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 1:
                    this.Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        public override void OnBackPressed()
        {
            new AlertDialog.Builder(this)
            .SetTitle("Do you really want to exit?")
            .SetNegativeButton("No", new EventHandler<DialogClickEventArgs>((dlgSender, dlgEvt) => { }))
            .SetPositiveButton("Yes", new EventHandler<DialogClickEventArgs>((dlgSender, dlgEvt) => { Finish(); })).Create().Show();
        }
    }
}