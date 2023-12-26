using HospitalMgrSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgrSystemUI.Models
{
    public class ConsultantDto
    {
        public List<Specialist> Specialists { get; set; }
        public Consultant Consultant { get; set; }
        public List<Consultant> listConsultants { get; set; }
        public string SearchValue { get; set; }
    }
}
