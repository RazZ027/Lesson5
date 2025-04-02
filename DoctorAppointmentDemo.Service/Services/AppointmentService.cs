using DoctorAppointmentDemo.Service.Interfaces;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;

namespace MyDoctorAppointment.Service.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IDataStorage<Appointment> _appointmentRepository;

        public AppointmentService(IDataStorage<Appointment> storage)
        {
            _appointmentRepository = storage;
        }

        public Appointment Create(Appointment appointment) => _appointmentRepository.Create(appointment);
        public Appointment? Get(int id) => _appointmentRepository.GetById(id);
        public IEnumerable<Appointment> GetAll() => _appointmentRepository.GetAll();
        public Appointment Update(int id, Appointment appointment) => _appointmentRepository.Update(id, appointment);
        public bool Delete(int id) => _appointmentRepository.Delete(id);
    }
}
