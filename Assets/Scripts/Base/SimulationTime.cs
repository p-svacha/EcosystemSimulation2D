using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationTime
{
    /// <summary>
    /// Year since the start of the game. Starts at 1.
    /// </summary>
    public int Year { get; private set; }
    /// <summary>
    /// Month of the year. Starts at 1.
    /// </summary>
    public int Month { get; private set; }
    /// <summary>
    /// Day of the quadrum. Starts at 1.
    /// </summary>
    public int Day { get; private set; }
    /// <summary>
    /// Hour of the day. Starts at 0.
    /// </summary>
    public int Hour { get; private set; }
    /// <summary>
    /// Hidden attribute between 0 and 1 that defines how much of the current hour has passed already.
    /// </summary>
    public float HourSplit { get; private set; }

    private const int MonthsPerYear = 4;
    private const int DaysPerMonth = 10;
    private const int HoursPerDay = 24;

    public SimulationTime(int year, int month, int day, int hour, float hourSplit = 0f)
    {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        HourSplit = hourSplit;
    }

    /// <summary>
    /// Returns a copy / timestamp from this time.
    /// </summary>
    public SimulationTime Copy() => new SimulationTime(Year, Month, Day, Hour, HourSplit);

    public void IncreaseTime(float hourSplitInterval)
    {
        HourSplit += hourSplitInterval;
        if(HourSplit >= 1)
        {
            HourSplit -= 1;
            Hour++;
            if(Hour >= HoursPerDay)
            {
                Hour = 0;
                Day++;
                if(Day > DaysPerMonth)
                {
                    Day = 1;
                    Month++;
                    if(Month > MonthsPerYear)
                    {
                        Month = 1;
                        Year++;
                    }
                }
            }
        }
    }


    public string FullString { get { return Day + "." + Month + "." + Year + " " + Hour + "h"; } }
    /// <summary>
    /// Amount of started months since the start of the game
    /// </summary>
    public int AbsoluteMonth { get { return (Year - 1) * MonthsPerYear + Month; } }
    /// <summary>
    /// Amount of started days since the start of the game
    /// </summary>
    public int AbsoluteDay { get { return (Year - 1) * MonthsPerYear * DaysPerMonth + (Month - 1) * DaysPerMonth + Day; } }
    /// <summary>
    /// Amount of started hours since the start of the game
    /// </summary>
    public int AbsoluteHour { get { return (Year - 1) * MonthsPerYear * DaysPerMonth * HoursPerDay + (Month - 1) * DaysPerMonth * HoursPerDay + (Day - 1) * HoursPerDay + Hour; } }
    /// <summary>
    /// Exact amount of hours since start of the game, including decimal values.
    /// </summary>
    public float ExactTime { get { return (Year - 1) * MonthsPerYear * DaysPerMonth * HoursPerDay + (Month - 1) * DaysPerMonth * HoursPerDay + (Day - 1) * HoursPerDay + (Hour - 1) + HourSplit; } }


}
