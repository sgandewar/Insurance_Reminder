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

namespace Insurance_Reminder
{
    [Activity(Label = "Reminder Details")]
    public class ReminderDetails : Activity
    {
        InsuranceReminderBO insurance;
        //TextView lblReadonlyID;
        TextView lblReadonlyCompanyName;
        TextView txtReadonlyPremiumAmount;
        TextView txtReadonlyDueDate;
        Button btnUpdate;
        Button btnDeleteTask;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ReminderDetails);
            insurance = (InsuranceReminderBO)Intent.GetParcelableExtra("SelectedItem");

            //lblReadonlyID = FindViewById<TextView>(Resource.Id.lblReadonlyID);
            lblReadonlyCompanyName = FindViewById<TextView>(Resource.Id.lblReadonlyCompanyName);
            txtReadonlyPremiumAmount = FindViewById<TextView>(Resource.Id.txtReadonlyPremiumAmount);
            txtReadonlyDueDate = FindViewById<TextView>(Resource.Id.txtReadonlyDueDate);
            btnDeleteTask = FindViewById<Button>(Resource.Id.btnDeleteTask);
            btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);

            //lblReadonlyID.Text = insurance.ID.ToString();
            lblReadonlyCompanyName.Text = insurance.Company_Name.ToString();
            txtReadonlyPremiumAmount.Text = insurance.Premium_Amount.ToString();
            txtReadonlyDueDate.Text = insurance.Due_Date.ToString();
            btnUpdate.Click += btnUpdate_Click;
            btnDeleteTask.Click += btnDeleteTask_Click;
        }

        void btnDeleteTask_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(Resource.String.ShowDeleteConfirmationMessage);
            builder.SetPositiveButton(Resource.String.DialogYesButton, DialogYesButtonClicked);
            builder.SetNegativeButton(Resource.String.DialogNoButton, DialogNoButtonClicked);
            builder.Show();
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(UpdateDetails));
            intent.PutExtra("UpdateObject", insurance);
            StartActivity(intent);
            this.Finish();
        }

        private void DialogYesButtonClicked(object sender, DialogClickEventArgs args)
        {
            try
            {
                InsuranceDataSource insuranceDataSource = new InsuranceDataSource(this);
                if (insuranceDataSource.DeleteInsuranceReminder(insurance))
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
    }
}