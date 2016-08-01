using Fiddler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: Fiddler.RequiredVersion("2.3.4.4")]

namespace HypermediaInspector
{
    public class HypermediaInspector : Inspector2, IResponseInspector2
    {        
        HypermediaViewer _myControl;
        HTTPHeaders _headers;
        private byte[] _entityBody;
        bool _contentOK;
      
        public bool bDirty
        {
            get
            {
                return false;
            }
        }

        public byte[] body
        {
            get
            {
                return _entityBody;
            }
            set
            {
                _entityBody = Fiddler.Utilities.Dupe(value);
                if (_entityBody != null)
                {
                    if (_contentOK)
                    {
                        Fiddler.Utilities.utilDecodeHTTPBody(_headers, ref _entityBody, false);
                        _myControl.JsonContent = System.Text.Encoding.UTF8.GetString(_entityBody);
                    }
                    else
                    {
                        _myControl.Lock();
                    }
                }
                else
                {
                    _myControl.Clear();
                }

            }
        }

        public bool bReadOnly
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        public HTTPResponseHeaders headers
        {
            get
            {
                // Header editing is not allowed
                return null;
            }
            set
            {
                _headers = value;
                _contentOK = false;
                if (_headers != null && _headers.Exists("Content-Type"))
                {
                    string content = _headers["Content-Type"].ToLower();
                    if (content.Contains("json"))
                    {
                        _contentOK = true;
                    }
                }
            }
        }

        public void CallApi(string url)
        {
            Uri requestUrl = new Uri(url);
            HTTPRequestHeaders headers = new HTTPRequestHeaders();
            headers.Add("Accept", "application/json");

            headers.HTTPMethod = "GET";
            headers.UriScheme = requestUrl.Scheme;
            headers.RequestPath = requestUrl.AbsolutePath;
            headers["HOST"] = requestUrl.Host;

            //FiddlerApplication.DoComposeFrom(headers, null);
            FiddlerApplication.oProxy.SendRequest(headers, null, null);
        }

        public override void AddToTab(System.Windows.Forms.TabPage o)
        {
            _myControl = new HypermediaViewer(this);
            o.Text = "Hypermedia";
            o.Controls.Add(_myControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        public void Clear()
        {
            _entityBody = null;
            _myControl.Clear();
        }

        public override int GetOrder()
        {
            return 0;
        }

        public override void SetFontSize(float flSizeInPoints)
        {
            if (_myControl != null)
                _myControl.Font = new Font(_myControl.Font.FontFamily, flSizeInPoints);
        }
    }
}
