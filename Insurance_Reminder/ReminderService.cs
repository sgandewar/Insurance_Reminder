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
using Android.Support.V4.App;

namespace Insurance_Reminder
{
[Service]
    public class ReminderService : WakeReminderIntentService
    {
    private static readonly int ButtonClickNotificationId = 1000;

        public ReminderService()
            : base("ReminderService")
        {

        }

        internal override void doReminderWork(Intent intent)
        {
            try
            {
                // These are the values that we want to pass to the next activity
                int rowId = (int)intent.Extras.GetLong(SQLiteHelper.Column_id);
                
                Bundle valuesForActivity = new Bundle();
                valuesForActivity.PutInt("rowid", rowId);
                

                // Create the PendingIntent with the back stack
                // When the user clicks the notification, SecondActivity will start up.
                Intent resultIntent = new Intent(this, typeof(AddNewReminder));
                resultIntent.PutExtras(valuesForActivity); // Pass some values to SecondActivity.

                TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
                stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(AddNewReminder)));
                stackBuilder.AddNextIntent(resultIntent);

                PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                // Build the notification
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                    .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
                    .SetContentIntent(resultPendingIntent) // start up this activity when the user clicks the intent.
                    .SetContentTitle("Reminder Notification") // Set the title
                    .SetNumber(rowId) // Display the count in the Content Info
                    .SetSmallIcon(Resource.Drawable.Icon) // This is the icon to display
                    .SetContentText(String.Format("Insurance Reminder Notification for Insurance ID {0}.", rowId)); // the message to display.

                // Finally publish the notification
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                
                notificationManager.Notify(ButtonClickNotificationId, builder.Build());

                



                //long rowId = intent.Extras.GetLong(SQLiteHelper.Column_id);
                //NotificationManager mgr = (NotificationManager)GetSystemService(NotificationService);
                //Intent notificationIntent = new Intent(this, typeof(AddNewReminder)); // To DO: Need to add Edit Screen class
                //notificationIntent.PutExtra(SQLiteHelper.Column_id, rowId);
                //PendingIntent pi = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.OneShot);
                //DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                //long ms = (long)(DateTime.UtcNow - epoch).TotalMilliseconds;
                //long result = ms;
                //Notification note = new Notification(Resource.Drawable.Icon, "Task Name", result);
                //note.SetLatestEventInfo(this, "Task Title", "Notify New Task Message", pi);
                ////note.Defaults |= NotificationDefaults.Sound;
                //note.Flags |= NotificationFlags.ShowLights;
                //// An issue could occur if user ever enters over 2,147,483,647 tasks. (Max int value). 
                //// I highly doubt this will ever happen. But is good to note. 
                //int id = (int)((long)rowId);
                //mgr.Notify(id, note);
            }
            catch (Exception ex)
            {

            }
        }
    }
}