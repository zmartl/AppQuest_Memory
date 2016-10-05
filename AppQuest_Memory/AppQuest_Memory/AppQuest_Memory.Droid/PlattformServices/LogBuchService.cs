using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppQuest_Memory.Droid.PlattformServices;
using AppQuest_Memory.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LogBuchService))]
namespace AppQuest_Memory.Droid.PlattformServices
{
    public class LogBuchService
        : ILogBuchService
    {
        public void OpenLogBuch(string task, string solution)
        {
            var context = Forms.Context;

            Intent intent = new Intent("ch.appquest.intent.LOG");

            if (context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly).Count == 0)
            {
                Toast.MakeText(context, "Logbook App not Installed", ToastLength.Long).Show();
                return;
            }

            // Achtung, je nach App wird etwas anderes eingetragen
            intent.PutExtra("ch.appquest.logmessage", $"{{  \"task\": \"{task}\", \"solution\": {solution}}}");

            context.StartActivity(intent);
        }
    }
}