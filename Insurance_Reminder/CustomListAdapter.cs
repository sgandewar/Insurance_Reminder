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
            DateTime dtDue = DateTime.Now;
            DateTime.TryParse(insurance.Due_Date, out dtDue);
            TimeSpan tsDueIn = dtDue.Subtract(DateTime.Now);
            String strTSDueIn = String.Empty;
            if (tsDueIn.Days < 1 && tsDueIn.Hours < 1)
                strTSDueIn = String.Format("{0} minutes", tsDueIn.Minutes.ToString());
            else if (tsDueIn.Days < 1)
                strTSDueIn = String.Format("{0} hours", tsDueIn.Hours.ToString());
            else if (tsDueIn.Days.Equals(1))
                strTSDueIn = String.Format("{0} day", tsDueIn.Days.ToString());
            else
                strTSDueIn = String.Format("{0} days", tsDueIn.Days.ToString());
            view.FindViewById<TextView>(Resource.Id.lblDueDate).Text = String.Format("Due in {0}", strTSDueIn);
            return view;
        }
    }
}