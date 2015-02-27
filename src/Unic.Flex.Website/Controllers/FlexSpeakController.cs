namespace Unic.Flex.Website.Controllers
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using System;

    public class FlexSpeakController : Controller
    {
        public ActionResult GetAsyncTasks()
        {
            var tasks = new AsyncTaks();
            tasks.Data.Add(new AsyncTask { Form = "my Form", Plug = "plug", Attemps = 4, LastTry = DateTime.Now.ToString("G") });
            tasks.Data.Add(new AsyncTask { Form = "second", Plug = "plug", Attemps = 2, LastTry = DateTime.Now.ToString("G") });

            this.Response.ContentType = "application/json";
            
            return this.Content(JsonConvert.SerializeObject(tasks));
        }

        [DataContract]
        private class AsyncTaks
        {
            public AsyncTaks()
            {
                this.Data = new List<AsyncTask>();
            }

            [DataMember(Name = "data", EmitDefaultValue = false)]
            public IList<AsyncTask> Data { get; private set; } 
        }

        [DataContract]
        private class AsyncTask
        {
            [DataMember(Name = "form", EmitDefaultValue = false)]
            public string Form { get; set; }

            [DataMember(Name = "plug", EmitDefaultValue = false)]
            public string Plug { get; set; }

            [DataMember(Name = "attemps", EmitDefaultValue = false)]
            public int Attemps { get; set; }

            [DataMember(Name = "last_try", EmitDefaultValue = false)]
            public string LastTry { get; set; }
        }
    }
}