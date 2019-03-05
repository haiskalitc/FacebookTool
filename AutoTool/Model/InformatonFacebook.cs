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
        private string _uid;
        public string UID
        {
            get { return _uid; }
            set
            {
                if (_uid != value)
                {
                    _uid = value;
                    RaisePropertyChanged("UID");
                }
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
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
        private string _cookie;
        public string Cookie
        {
            get { return _cookie; }
            set
            {
                if (_cookie != value)
                {
                    _cookie = value;
                    RaisePropertyChanged("Cookie");
                }
            }
        }
        private int _friends;
        public int Friends
        {
            get { return _friends; }
            set
            {
                if (_friends != value)
                {
                    _friends = value;
                    RaisePropertyChanged("Friends");
                }
            }
        }
        private int _groups;
        public int Groups
        {
            get { return _groups; }
            set
            {
                if (_groups != value)
                {
                    _groups = value;
                    RaisePropertyChanged("Groups");
                }
            }
        }
        private int _fanpages;
        public int Fanpages
        {
            get { return _fanpages; }
            set
            {
                if (_fanpages != value)
                {
                    _fanpages = value;
                    RaisePropertyChanged("Fanpages");
                }
            }
        }
        private string _birthday;
        public string BirthDay
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    RaisePropertyChanged("BirthDay");
                }
            }
        }
        private string _backup;
        public string Backup
        {
            get { return _backup; }
            set
            {
                if (_backup != value)
                {
                    _backup = value;
                    RaisePropertyChanged("Backup");
                }
            }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }
        private bool _ischeck;
        public bool IsCheck
        {
            get { return _ischeck; }
            set
            {
                if (_ischeck != value)
                {
                    _ischeck = value;
                    RaisePropertyChanged("IsCheck");
                }
            }
        }
        private bool _isBackup;
        public bool IsBackup
        {
            get { return _isBackup; }
            set
            {
                if (_isBackup != value)
                {
                    _isBackup = value;
                    RaisePropertyChanged("IsBackup");
                }
            }
        }
    }
}
