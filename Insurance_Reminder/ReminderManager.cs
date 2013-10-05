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
using Java.Lang;
using Java.Util;

namespace Insurance_Reminder
{
    class ReminderManager
    {
        private Context mContext;
        private AlarmManager mAlarmManager;
        long repeatTime = 0;
        public ReminderManager(Context context)
        {
            mContext = context;
            mAlarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
        }

        public void setReminder(Long taskId, Calendar when, string strFreq)
        {


            Intent i = new Intent(mContext, typeof(OnAlarmReceiver));
            i.PutExtra(SQLiteHelper.Column_id, taskId);

            PendingIntent pi = PendingIntent.GetBroadcast(mContext, 0, i, PendingIntentFlags.OneShot);
            if (strFreq.Equals("One Time"))
            {
                repeatTime = 0;
            }
            else if (strFreq.Equals("Weekly"))
            {
                repeatTime = AlarmManager.IntervalDay * 7;
            }
            else if (strFreq.Equals("Monthly"))
            {
                repeatTime = AlarmManager.IntervalDay * 30;
            }
            else if(strFreq.Equals("Quaterly"))
            {
                repeatTime = AlarmManager.IntervalDay * 90;
            }
            else if(strFreq.Equals("Half-yearly"))
            {
                repeatTime = AlarmManager.IntervalDay *180;
            }
            else
            {
                repeatTime = AlarmManager.IntervalDay * 365;
            
            }

            mAlarmManager.SetRepeating(AlarmType.RtcWakeup, when.TimeInMillis, repeatTime, pi);

           // mAlarmManager.Set(AlarmType.RtcWakeup, when.TimeInMillis, pi);


        }
    }


}