using MyDoctorAppointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentDemo.Service.Interfaces
{
    public interface IPatientService
    {
        Patient Create(Patient patient);
        IEnumerable<Patient> GetAll();
        Patient? Get(int id);
        bool Delete(int id);
        Patient Update(int id, Patient patient);
    }
}
