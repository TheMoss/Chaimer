using System;
using System.Reactive.Linq;
using System.Timers;
using ReactiveUI;

namespace Chaimer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private TimeOnly _steepTime;
    public TimeOnly SteepTime
    {
        get => _steepTime;
        set => this.RaiseAndSetIfChanged(ref _steepTime, value);
    }

    private TimeOnly _firstSteepTime;

    public TimeOnly FirstSteepTime
    {
        get => _firstSteepTime;
        set => this.RaiseAndSetIfChanged(ref _firstSteepTime, value);
    }
    
    private int _steepIncrement;
    public int SteepIncrement {
        get => _steepIncrement;
        set => this.RaiseAndSetIfChanged(ref _steepIncrement, value);
    }

    private int _steepCount;
    private int _steepTimeInSeconds;
    private Timer _steepTimer = new Timer();
    
    public int SteepCount
    {
        get => _steepCount;
        set => this.RaiseAndSetIfChanged(ref _steepCount, value);
    }

    private long _elapsedTime;
    public long ElapsedTime
    
    {
        get => _elapsedTime;
        set => this.RaiseAndSetIfChanged(ref _elapsedTime, value);
    }
    

    public void FirstSteepAddSeconds()
    {
       FirstSteepTime = FirstSteepTime.Add(new TimeSpan(hours: 0, minutes: 0, seconds: 1));
       if (SteepCount == 0) 
       {
           SteepTime = FirstSteepTime;
       }
       else
       {
           SteepTime = FirstSteepTime.Add(new TimeSpan(hours:0, minutes:0, seconds:SteepIncrement));
       }
    }

    public void FirstSteepSubstractSeconds()
    {
        FirstSteepTime = FirstSteepTime.Add(new TimeSpan(hours: 0, minutes: 0, seconds: -1));
        if (SteepCount == 0)
        {
            SteepTime = FirstSteepTime;
        }
        else
        {
            SteepTime = FirstSteepTime.Add(new TimeSpan(hours:0, minutes:0, seconds:SteepIncrement));
        }
    }

    public void SteepIncrementAddSeconds()
    {
        SteepIncrement++;
        if (SteepCount > 0)
        {
            SteepTime = FirstSteepTime.Add(new TimeSpan(hours: 0, minutes: 0, seconds: SteepIncrement * SteepCount));
        }
    }

    public void SteepIncrementSubstractSeconds()
    {
        SteepIncrement--;
        if (SteepCount > 0)
        {
            SteepTime = FirstSteepTime.Add(new TimeSpan(hours: 0, minutes: 0, seconds: SteepIncrement * SteepCount));
        }
    }
    public void ReactiveTimer()
    {
        if (SteepCount == 0)
        {
            SteepTime = FirstSteepTime;
            _steepTimeInSeconds = SteepTime.Second;
        }
        else
        {
            SteepTime = FirstSteepTime.Add(new TimeSpan(hours: 0, minutes: 0, seconds: SteepIncrement * SteepCount));
            _steepTimeInSeconds = SteepTime.Second;
        }

        Observable
            .Interval(TimeSpan.FromSeconds(1))
            .Take(_steepTimeInSeconds)
            .Select(x => _steepTimeInSeconds - x - 1)
            .StartWith(_steepTimeInSeconds)
            .Finally(() =>SteepCount++)
            .Subscribe(x => ElapsedTime = x);
    }

}