﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CumulativePart1.Models;
using MySql.Data.MySqlClient;

namespace CumulativePart1.Controllers
{   
    public class TeacherDataController : ApiController
    {
        //database context class, allows access to mysql database
        private SchooldbContext schooldb = new SchooldbContext();
        /// <summary>
        /// this controller returns a list of teachers from the schooldb database
        /// </summary>
        /// <example>GET api/Teacherdata</example>
        /// <returns>
        /// A list of Teacher data: teacherid, teacher first name, teacher last name, hiredate
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData")]
            public IEnumerable<Teacher> ListTeachers()
            {
                //Create an instance of a connection
                MySqlConnection Conn = schooldb.AccessDatabase();

                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Select * from teachers";

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Create an empty list of Teachers
                List<Teacher> Teachers = new List<Teacher> { };

                //Loop Through Each Row the Result Set
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int TeacherId = (int)ResultSet["teacherid"];
                    string TeacherFname = ResultSet["teacherfname"].ToString();
                    string TeacherLname = ResultSet["teacherlname"].ToString();
                    string HireDate = ResultSet["hiredate"].ToString();

                    // assign teacher data to newteacher
                    Teacher NewTeacher = new Teacher();
                    NewTeacher.teacherid = TeacherId;
                    NewTeacher.teacherfname = TeacherFname;
                    NewTeacher.teacherlname = TeacherLname;
                    NewTeacher.hiredate = HireDate;

                    //Add the teacher data to the List
                    Teachers.Add(NewTeacher);
                }

                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();

                //Return the final list of teacher data
                return Teachers;
            }

        /// <summary>
        /// this controller returns data for a specific teacher from the schooldb database
        /// </summary>
        /// <example>GET api/findteacher/{id}</example>
        /// <returns>
        /// teacher data: teacherid, teacher first name, teacher last name, hiredate
        /// </returns>

        [HttpGet]
        [Route("api/findteacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            //create new teacher object
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = schooldb.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherid = (int)ResultSet["teacherid"];
                string teacherfname = ResultSet["teacherfname"].ToString();
                string teacherlname = ResultSet["teacherlname"].ToString();
                string hiredate = ResultSet["hiredate"].ToString();

                //assign teacher data to newteacher object
                NewTeacher.teacherid = teacherid;
                NewTeacher.teacherfname = teacherfname;
                NewTeacher.teacherlname = teacherlname;
                NewTeacher.hiredate = hiredate;
            }
            //close database connection
            Conn.Close();
            //return new teacher object
            return NewTeacher;
        }

    }
}