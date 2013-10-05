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
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ReminderDetails);
            insurance = (InsuranceReminderBO)Intent.GetParcelableExtra("SelectedItem");

            //lblReadonlyID = FindViewById<TextView>(Resource.Id.lblReadonlyID);
            lblReadonlyCompanyName = FindViewById<TextView>(Resource.Id.lblReadonlyCompanyName);
            txtReadonlyPremiumAmount = FindViewById<TextView>(Resource.Id.txtReadonlyPremiumAmount);
            txtReadonlyDueDate = FindViewById<TextView>(Resource.Id.txtReadonlyDueDate);

            //lblReadonlyID.Text = insurance.ID.ToString();
            lblReadonlyCompanyName.Text = insurance.Company_Name.ToString();
            txtReadonlyPremiumAmount.Text = insurance.Premium_Amount.ToString();
            txtReadonlyDueDate.Text = insurance.Due_Date.ToString();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var MenuItemUpdate = menu.Add(0, 1, 1, Resource.String.UpdateDetails);
            var MenuItemDelete = menu.Add(0, 2, 2, Resource.String.Delete);
            MenuItemUpdate.SetIcon(Resource.Drawable.Update);
            MenuItemDelete.SetIcon(Resource.Drawable.delete);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 1:
                    var intent = new Intent(this, typeof(UpdateDetails));
                    intent.PutExtra("UpdateObject", insurance);
                    StartActivity(intent);
                    this.Finish();
                    return true;

                case 2:
                    bool bflag = DeleteReminder();
                    if (bflag)
                    {
                        StartActivity(typeof(MainScreen));
                        this.Finish();
                        return true;
                    }
                    else
                        return false;
            
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected bool DeleteReminder()
        {
            bool brtnVal = false;
            try
            {
                InsuranceDataSource insuranceDataSource = new InsuranceDataSource(this);
                brtnVal = insuranceDataSource.DeleteInsuranceReminder(insurance);
            }
            catch (Exception ex)
            {
            }
            return brtnVal;
        }
    }
}