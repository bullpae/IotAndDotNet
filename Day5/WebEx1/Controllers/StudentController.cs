using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebEx1.Controllers
{
    public class StudentController : ApiController
    {
        public IHttpActionResult Get()
        {
            using (var db = new DBContext())
            {
                var studentList = db.Students.ToList();
                return Ok(studentList);
            }
        }

        public IHttpActionResult Get(string studentNo)
        {
            using (var db = new DBContext())
            {
                var student = db.Students.SingleOrDefault(p => p.StudentNo == studentNo);
                if (student != null)
                {
                    return Ok(student);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public IHttpActionResult Post(Student student)
        {
            using (var db = new DBContext())
            {
                db.Students.Add(student);
                db.SaveChanges();
                return Ok();
            }
        }

        public IHttpActionResult Put(Student student)
        {
            using (var db = new DBContext())
            {
                var oldStudent = db.Students.SingleOrDefault(p => p.StudentNo == student.StudentNo);

                if (oldStudent != null)
                {
                    oldStudent.Name = student.Name;
                    oldStudent.Age = student.Age;
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public IHttpActionResult Delete(string studentNo)
        {
            using (var db = new DBContext())
            {
                var oldStudent = db.Students.SingleOrDefault(p => p.StudentNo == studentNo);
                if (oldStudent != null)
                {
                    db.Students.Remove(oldStudent);
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
