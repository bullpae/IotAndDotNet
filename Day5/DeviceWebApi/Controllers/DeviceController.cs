using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeviceWebApi.Controllers
{
    public class DeviceController : ApiController
    {

        public IHttpActionResult Get()
        {
            using (var db = new DBContext())
            {
                var deviceLogList = db.DeviceLogs.ToList();
                return Ok(deviceLogList);
            }
        }

        public IHttpActionResult Post(DeviceLog log)
        {
            using (var db = new DBContext())
            {
                log.LogDate = DateTime.Now;
                db.DeviceLogs.Add(log);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}
