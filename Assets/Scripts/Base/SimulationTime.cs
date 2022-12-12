using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationTime
{
    public float AbsoluteTime { get; private set; }
    /// <summary>
    /// Amount of full years
    /// </summary>
    public int Year => (int)(AbsoluteTime / HoursPerDay / DaysPerMonth / MonthsPerYear);
    /// <summary>
    /// Amount of full months of the Year
    /// </summary>
    public int Month => AbsoluteMonth % MonthsPerYear;
    /// <summary>
    /// Amount of full days of the month
    /// </summary>
    public int Day => AbsoluteDay % DaysPerMonth;
    /// <summary>
    /// Amount of full and partial hours of the day
    /// </summary>
    public float Hour => AbsoluteTime % HoursPerDay;

    private const int MonthsPerYear = 4;
    private const int DaysPerMonth = 10;
    private const int HoursPerDay = 24;

    private readonly string[] MonthNames = { "January", "April", "Juli", "October"};
    /// <summary>
    /// SimulationTime initialized at 0.
    /// </summary>
    public SimulationTime() { }

    public SimulationTime(int year, int month, int day, int hour, float hourSplit = 0f)
    {
        AbsoluteTime = (year * MonthsPerYear * DaysPerMonth * HoursPerDay) + (month * DaysPerMonth * HoursPerDay) + (day * HoursPerDay) + hour + hourSplit;
    }

    public SimulationTime(float absoluteTime)
    {
        AbsoluteTime = absoluteTime;
    }

    /// <summary>
    /// Returns a copy / timestamp from this time.
    /// </summary>
    public SimulationTime Copy() => new SimulationTime(AbsoluteTime);


    public void IncreaseTime(float hourSplitInterval)
    {
        AbsoluteTime += hourSplitInterval;
    }

    public void Reset()
    {
        AbsoluteTime = 0;
    }


    public string DateString { get { return (Day + 1) + HelperFunctions.GetOrdinalSuffix(Day + 1) + " of " + MonthNames[Month] + ", Year " + Year + " | " + (int)Hour + "h"; } }

    /// <summary>
    /// Amount of fully completed months
    /// </summary>
    public int AbsoluteMonth => (int)(AbsoluteTime / DaysPerMonth / HoursPerDay);
    /// <summary>
    /// Amount of fully completed days
    /// </summary>
    public int AbsoluteDay => (int)(AbsoluteTime / HoursPerDay);
    /// <summary>
    /// Amount of started hours since the start of the game
    /// </summary>
    public int AbsoluteHour => (int)AbsoluteTime;

    #region Operators

    public static float operator +(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime + f.AbsoluteTime;
    }
    public static float operator -(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime - f.AbsoluteTime;
    }
    public static float operator /(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime / f.AbsoluteTime;
    }
    public static float operator *(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime * f.AbsoluteTime;
    }
    public static bool operator <(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime < f.AbsoluteTime;
    }
    public static bool operator >(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime > f.AbsoluteTime;
    }
    public static bool operator <=(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime <= f.AbsoluteTime;
    }
    public static bool operator >=(SimulationTime l, SimulationTime f)
    {
        return l.AbsoluteTime >= f.AbsoluteTime;
    }

    public static bool operator <(SimulationTime l, float f)
    {
        return l.AbsoluteTime < f;
    }
    public static bool operator >(SimulationTime l, float f)
    {
        return l.AbsoluteTime > f;
    }
    public static bool operator <=(SimulationTime l, float f)
    {
        return l.AbsoluteTime <= f;
    }
    public static bool operator >=(SimulationTime l, float f)
    {
        return l.AbsoluteTime >= f;
    }

    #endregion

    public override string ToString()
    {
        string text = "";
        if (Year > 0) text += Year + " Years ";
        if (Month > 0) text += Month + " Months ";
        if (Day > 0) text += Day + " Days ";
        if (Hour != 0) text += Hour.ToString("F1") + " Hours";
        if (text == "") return "0 Hours";
        return text;
    }


}
