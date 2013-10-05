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
using System.Runtime.CompilerServices;

namespace Insurance_Reminder
{
   
    public abstract class WakeReminderIntentService : IntentService
    {
        internal abstract void doReminderWork(Intent intent);

        public const string LOCK_NAME_STATIC = "InsuranceReminder.Static";
        private static PowerManager.WakeLock lockStatic = null;

        public static void acquireStaticLock(Context context)
        {
            try
            {

                getLock(context).Acquire();
            }
            catch (Exception ex)
            {
            }
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static PowerManager.WakeLock getLock(Context context)
        {
            if (lockStatic == null)
            {
                PowerManager mgr = (PowerManager)context.GetSystemService(Context.PowerService);
                lockStatic = mgr.NewWakeLock(WakeLockFlags.Partial, LOCK_NAME_STATIC);
                lockStatic.SetReferenceCounted(true);
            }
            return (lockStatic);
        }
        public WakeReminderIntentService()
            : base("WakeReminderIntentService")
        {
        }

        public WakeReminderIntentService(String name) : base(name)
        {
            
        }


        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                doReminderWork(intent);
            }
            finally
            {
                getLock(this).Release();
            }
        }
    }
}