using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CRUD_JQUERY
{ 
    public class CSVFormatter : MediaTypeFormatter
    {
        public CSVFormatter()
        {
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv"));
        }
        public CSVFormatter(MediaTypeMapping mediaTypeMapping) : this()
        {
            this.MediaTypeMappings.Add(mediaTypeMapping);
        }
        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            //List mi ?
            return type.GetInterfaces().Any(x => x == typeof(IEnumerable));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.Add("Content-Disposition", "attachment; filename=" + Guid.NewGuid().ToString() + ".csv");
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            myWriteStream(type, value, writeStream);
            var task = new TaskCompletionSource<int>();
            task.SetResult(0);
            return task.Task;
        }
        private void myWriteStream(Type type, object value, Stream stream)
        {

            Type itemType = type.GetGenericArguments()[0];

            StringWriter _stringWriter = new StringWriter();

            _stringWriter.WriteLine(
                string.Join<string>(
                    ",", itemType.GetProperties().Select(x => x.Name)
                )
            );

            foreach (var obj in (IEnumerable<object>)value)
            {

                var vals = obj.GetType().GetProperties().Select(
                    pi => new {
                        Value = pi.GetValue(obj, null)
                    }
                );

                string _valueLine = string.Empty;

                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {
                        var _val = val.Value.ToString();

                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");

                        _valueLine = string.Concat(_valueLine, _val, ",");
                    }
                    else
                    {
                        _valueLine = string.Concat(_valueLine, string.Empty, ",");
                    }
                }

                _stringWriter.WriteLine(_valueLine.TrimEnd(','));
            }

            var streamWriter = new StreamWriter(stream);
            streamWriter.Write(_stringWriter.ToString());
            streamWriter.Close();
        }
    }
}