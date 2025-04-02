using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDoctorAppointment.Domain.Entities;

namespace MyDoctorAppointment.Data.Interfaces
{
    public interface IDataStorage<T> where T : Auditable
    {
        T Create(T source);
        T? GetById(int id);
        T Update(int id, T source);
        IEnumerable<T> GetAll();
        bool Delete(int id);
    }
}
