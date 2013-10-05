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
    [BroadcastReceiver]
    public class OnAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
                long rowid = intent.Extras.GetLong(SQLiteHelper.Column_id);
               
                WakeReminderIntentService.acquireStaticLock(context);

                Intent i = new Intent(context, typeof(ReminderService));
                
                i.PutExtra(SQLiteHelper.Column_id, rowid);

                context.StartService(i);
            }
            catch (Exception ex)
            {
            }
        }
        
    }
}