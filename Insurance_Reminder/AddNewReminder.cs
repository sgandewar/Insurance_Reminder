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
using Java.Util;


namespace Insurance_Reminder
{
    [Activity(Label = "Add New Reminder")]
    public class AddNewReminder : Activity
    {
        private Button btnAdd;
        private EditText txtCompanyName;
        private EditText txtPremiumAmount;
        private Calendar mCalendar;

        private long mRowId;
        
        //Date
        private TextView dateDisplay;
        private Button pickDate;
        private DateTime date;
        private const int DATE_DIALOG_ID = 0;
        //Date
        
        //Time
        private TextView time_display;
        private Button pick_button;
        private int hour;
        private int minute;
        private const int TIME_DIALOG_ID = 1;
        //Time
        
        // Frequency
        private TextView freqdisplay;
        private Button pickfreq;
        private const int FREQUENCY_DIALOG_ID = 2;
        // Frequency
        #region Common Code
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddNewReminder);

            btnAdd = FindViewById<Button>(Resource.Id.btnAddTask);
            txtCompanyName = FindViewById<EditText>(Resource.Id.txtCompanyName);
            txtPremiumAmount = FindViewById<EditText>(Resource.Id.txtPremiumAmount);
            mCalendar = Calendar.GetInstance(Java.Util.TimeZone.Default);

            
            pickfreq = FindViewById<Button>(Resource.Id.pickfreq);
            pickfreq.Click += (o, e) => ShowDialog( FREQUENCY_DIALOG_ID);

            mRowId = savedInstanceState != null ? savedInstanceState.GetLong(SQLiteHelper.Column_id) : 0;

            //Time
            time_display = FindViewById<TextView>(Resource.Id.timeDisplay);
            pick_button = FindViewById<Button>(Resource.Id.pickTime);
            pick_button.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);
            
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;
            UpdateTimeDisplay();
            //Time


            //Date 
            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            pickDate = FindViewById<Button>(Resource.Id.pickDate);
            date = DateTime.Today;
            btnAdd.Click += btnAdd_Click;
            pickDate.Click += delegate { ShowDialog(DATE_DIALOG_ID); };
            UpdateDateDisplay();
            //Date

            

        }
        protected void onResume()
        {
            setRowIdFromIntent();
        }
        private void setRowIdFromIntent()
        {
            if (mRowId == -1)
            {
                Bundle extras = Intent.Extras;
                mRowId = extras != null ? extras.GetLong(SQLiteHelper.Column_id)
                                        : -1;

            }
        }
        //protected void onSaveInstanceState(Bundle outState)
        //{
        //    base.OnSaveInstanceState(outState);
        //    outState.PutLong(SQLiteHelper.Column_id, mRowId);
        //}
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG_ID:
                    return new DatePickerDialog(this, OnDateSet, date.Year, date.Month - 1, date.Day);
                case TIME_DIALOG_ID:
                    return new TimePickerDialog(this, TimePickerCallback, hour, minute, false);
                case FREQUENCY_DIALOG_ID:
                    var builder = new AlertDialog.Builder(this);
                    //builder.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
                    builder.SetTitle("Select Frequency");
                    builder.SetSingleChoiceItems(Resource.Array.list_dialog_items, -1, ListClicked);
                    builder.SetPositiveButton("Ok", OkClicked);
                    builder.SetNegativeButton("Cancel", CancelClicked);
                    return builder.Create();
            }
            return null;
        }
        #endregion


        #region Frequency
        private void OkClicked(object sender, DialogClickEventArgs args)
        {
            var dialog = (AlertDialog)sender;
        }
        private void CancelClicked(object sender, DialogClickEventArgs args)
        {
        }
        private void ListClicked(object sender, DialogClickEventArgs args)
        {
            var items = Resources.GetStringArray(Resource.Array.list_dialog_items);
            pickfreq.Text = items[args.Which];
            //Toast.MakeText(this, string.Format("You've selected: {0}, {1}", args.Which, items[args.Which]), ToastLength.Short).Show();
        }
        #endregion

       
        #region Date
        private void UpdateDateDisplay()
        {
            dateDisplay.Text = date.ToString("d");
        }
        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.date = e.Date;
            mCalendar.Set(CalendarField.Year, e.Year);
            mCalendar.Set(CalendarField.Month, e.MonthOfYear);
            mCalendar.Set(CalendarField.DayOfMonth, e.DayOfMonth);
            UpdateDateDisplay();
        }
        #endregion
       
        
        
        
        #region TIme
        private void UpdateTimeDisplay()
        {
            string time = string.Format("{0}:{1}", hour, minute.ToString().PadLeft(2, '0'));
            time_display.Text = time;
        }
        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            hour = e.HourOfDay;
            minute = e.Minute;

            mCalendar.Set(CalendarField.HourOfDay, hour);
            mCalendar.Set(CalendarField.Minute, minute);
            UpdateTimeDisplay();
        }
        #endregion
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutLong(SQLiteHelper.Column_id, mRowId);
        }
       
       
        void btnAdd_Click(object sender, EventArgs e)
        {
            InsuranceReminderBO insure = new InsuranceReminderBO();
            insure.Company_Name = txtCompanyName.Text;
            insure.Premium_Amount = txtPremiumAmount.Text;
            insure.Due_Date = dateDisplay.Text;
            insure.Time_Due = dateDisplay.Text;
            insure.Frequency = pickfreq.Text;
            InsuranceDataSource insureDataSource = new InsuranceDataSource(this);
            mRowId = insureDataSource.CreateInsuranceReminder(insure);
            if (mRowId > -1)
            {
                Toast.MakeText(this, "Success", ToastLength.Short).Show();
                StartActivity(typeof(MainScreen));
                new ReminderManager(this).setReminder((Java.Lang.Long)mRowId, mCalendar, insure.Frequency);
            }
            else
                Toast.MakeText(this, "Error", ToastLength.Short).Show();

            Finish();
            
        }

       

    }
}