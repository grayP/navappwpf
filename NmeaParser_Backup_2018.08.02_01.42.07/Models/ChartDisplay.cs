using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NmeaParser.Helper;
using Telerik.Windows.Data;

namespace NmeaParser.Models
{
    public class ChartDisplay:  INotifyPropertyChanged
    {
        public RadObservableCollection<ChartBusinessObject> SlowCollection { get; set; } =
            new RadObservableCollection<ChartBusinessObject>();
        public RadObservableCollection<ChartBusinessObject> FastCollection { get; set; } =
            new RadObservableCollection<ChartBusinessObject>();

        private RadObservableCollection<ChartBusinessObject> _chartDataFast; 
        public RadObservableCollection<ChartBusinessObject> ChartDataFast
        {
            get => _chartDataFast;
            set
            {
                if (this._chartDataFast == value) return;
                this._chartDataFast = value;
                NotifyPropertyChanged();
            }
        }
        private RadObservableCollection<ChartBusinessObject> _chartDataSlow;
        public RadObservableCollection<ChartBusinessObject> ChartDataSlow
        {
            get => this._chartDataSlow;
            set
            {
                if (this._chartDataSlow == value) return;
                this._chartDataSlow = value;
                NotifyPropertyChanged();
            }
        }


        private double _yaxisMinimum;
        public double YaxisMinimum
        {
            get => _yaxisMinimum;
            set
            {
                if (this._yaxisMinimum == value) return;
                this._yaxisMinimum = value;
                NotifyPropertyChanged();
            }
        }
        private double _yaxisMaximum;
        public double YaxisMaximum
        {
            get => _yaxisMaximum;
            set
            {
                if (this._yaxisMaximum == value) return;
                this._yaxisMaximum = value;
                NotifyPropertyChanged();
            }
        }
        private DateTime _xaxisMinimum;
        public DateTime XaxisMinimum
        {
            get => _xaxisMinimum;
            set
            {
                if (this._xaxisMinimum == value) return;
                this._xaxisMinimum = value;
                NotifyPropertyChanged();
            }
        }
       #region methods

        public ChartDisplay()
        {

        }

 
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void SetmaxMin(int RoundTo)
        {
            if (!FastCollection.Any()) return;
            var newMin = ExtensionMethods.RoundToNearest((int)FastCollection.Min(x => x.Value), 10.0) - 2*RoundTo;
            if (newMin < YaxisMinimum || newMin > YaxisMinimum + 19) YaxisMinimum = newMin;
            YaxisMinimum = YaxisMinimum + RoundTo;
            var newMax = ExtensionMethods.RoundToNearest((int)FastCollection.Max(x => x.Value), RoundTo) + 2* RoundTo;
            if (newMax > YaxisMaximum || newMax < YaxisMaximum - (2* RoundTo-1)) YaxisMaximum = newMax;
            XaxisMinimum = FastCollection.FirstOrDefault().ReadingDateTime;
        }
    }
}
