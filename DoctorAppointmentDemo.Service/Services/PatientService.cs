using DoctorAppointmentDemo.Service.Interfaces;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;

namespace MyDoctorAppointment.Service.Services
{
    public class PatientService : IPatientService
    {
        private readonly IDataStorage<Patient> _patientRepository;

        public PatientService(IDataStorage<Patient> storage)
        {
            _patientRepository = storage;
        }

        public Patient Create(Patient patient) => _patientRepository.Create(patient);
        public Patient? Get(int id) => _patientRepository.GetById(id);
        public IEnumerable<Patient> GetAll() => _patientRepository.GetAll();
        public Patient Update(int id, Patient patient) => _patientRepository.Update(id, patient);
        public bool Delete(int id) => _patientRepository.Delete(id);
    }
}
