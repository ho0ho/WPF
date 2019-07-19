using System;
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
        public PointViewModel(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        #region X

        private float x;
        public float X
        {
            get { return x; }
            set
            {
                if (Numerics.FloatEquals(x, value)) return;
                x = value;
                RaisePropertyChanged("X");
            }
        }

        #endregion

        #region Y

        private float y;
        public float Y
        {
            get { return y; }
            set
            {
                if (Numerics.FloatEquals(y, value)) return;
                y = value;
                RaisePropertyChanged("Y");
            }
        }

        #endregion
    }

    
}
