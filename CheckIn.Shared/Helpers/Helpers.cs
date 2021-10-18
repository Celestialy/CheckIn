using System;
using System.Collections.Generic;
using System.Text;

namespace CheckIn.Shared.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// checks if array is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// Format timespan from 4:42:21 to 4 timer 42 minutter 21 sekunder
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToReadableTime(this TimeSpan time)
        {
            string hoursText = string.Empty;
            string minutesText = string.Empty;
            string secondsText = time.Seconds + " sekund";
            if (time.Hours != 0)
            {
                hoursText = time.Hours + " time";

                if (time.Hours != 1)
                    hoursText += "r";
                hoursText += ", ";
            }

            if (time.Minutes != 0 || hoursText != string.Empty)
            {
                minutesText = time.Minutes + " minut";
                if (time.Minutes != 1)
                    minutesText += "ter";
                minutesText += ", ";
            }
            if (time.Seconds != 1)
                secondsText += "er";

            return hoursText + minutesText + secondsText;
        }
    }
}
