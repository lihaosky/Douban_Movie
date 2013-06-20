using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    class BoolObject
    {
        private bool _isLoading;
        public bool isLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
            }
        }
        private bool _isLoaded;
        public bool isLoaded
        {
            get
            {
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;
            }
        }
        public BoolObject(bool loading, bool loaded)
        {
            _isLoaded = loaded;
            _isLoading = loading;
        }
    }
}
