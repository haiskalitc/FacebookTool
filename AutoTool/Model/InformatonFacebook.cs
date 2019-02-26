using AutoTool.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Model
{
    public class InformatonFacebook : BaseModel.BindableBase
    {
        // Thực inotify cho các element
        private string _token;
        public string Token
        {
            get { return _token; }
            set
            {
                if (_token != value)
                {
                    _token = value;
                    RaisePropertyChanged("Token");
                }
            }
        }

    }
}
