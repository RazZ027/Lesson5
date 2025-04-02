using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Service.Interfaces;

namespace MyDoctorAppointment.Service.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDataStorage<Doctor> _doctorRepository;

        public DoctorService(IDataStorage<Doctor> storage)
        {
            _doctorRepository = storage;
        }

        public Doctor Create(Doctor doctor) => _doctorRepository.Create(doctor);
        public Doctor? Get(int id) => _doctorRepository.GetById(id);
        public IEnumerable<Doctor> GetAll() => _doctorRepository.GetAll();
        public Doctor Update(int id, Doctor doctor) => _doctorRepository.Update(id, doctor);
        public bool Delete(int id) => _doctorRepository.Delete(id);
    }
}
