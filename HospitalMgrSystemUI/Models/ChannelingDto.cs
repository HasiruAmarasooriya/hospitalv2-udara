using HospitalMgrSystem.Model;
using System;
using System.Collections.Generic;

namespace HospitalMgrSystemUI.Models
{
    public class ChannelingDto
    {
        public int chID { get; set; }
        public List<Patient> PatientList { get; set; }
        public List<Channeling> ChannelingList { get; set; }
        public List<Consultant> listConsultants { get; set; }

        public List<ChannelingSchedule> listChannelingSchedule { get; set; }
        public int PatientID { get; set; }
        public int ConsultantID { get; set; }
        public Channeling channeling { get; set; }
        public int ChannelingScheduleID { get; set; }
    }
}
