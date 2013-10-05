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
    public class CustomListAdapter : BaseAdapter<InsuranceReminderBO>
    {
        List<InsuranceReminderBO> insuranceList;
        Activity context;
        public CustomListAdapter(Activity context, List<InsuranceReminderBO> insuranceList)
            : base()
        {
            this.context = context;
            this.insuranceList = insuranceList;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override InsuranceReminderBO this[int position]
        {
            get { return insuranceList[position]; }
        }

        public override int Count
        {
            get { return insuranceList.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var insurance = insuranceList[position];
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.list_row, null);
            //view.FindViewById<TextView>(Resource.Id.lblID).Text = insurance.ID.ToString();
            view.FindViewById<TextView>(Resource.Id.lblCompanyName).Text = insurance.Company_Name;
            view.FindViewById<TextView>(Resource.Id.lblPremiumAmount).Text = insurance.Premium_Amount;
            view.FindViewById<TextView>(Resource.Id.lblDueDate).Text = insurance.Due_Date;
            return view;
        }
    }
}