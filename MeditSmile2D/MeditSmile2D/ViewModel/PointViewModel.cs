﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MeditSmile2D.Common;

namespace MeditSmile2D.ViewModel
{
    public class PointViewModel : ViewModelBase
    {
        public PointViewModel(double x, double y, int s)
        {
            this.x = x;
            this.y = y;
            this.s = s;
        }

        #region X

        private double x;
        public double X
        {
            get { return x; }
            set
            {
                if (Numerics.DoubleEquals(x, value)) return;
                x = value;
                RaisePropertyChanged("X");
            }
        }

        #endregion

        #region Y

        private double y;
        public double Y
        {
            get { return y; }
            set
            {
                if (Numerics.DoubleEquals(y, value)) return;
                y = value;
                RaisePropertyChanged("Y");
            }
        }

        #endregion

        private int s;
        public int S
        {
            get { return s; }
            set { s = value; }
        }

    }

    
}
