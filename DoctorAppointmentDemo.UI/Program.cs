using DoctorAppointmentDemo.Data.Repositories;
using DoctorAppointmentDemo.Service.Interfaces;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Data.Repositories;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using MyDoctorAppointment.Service.Interfaces;
using MyDoctorAppointment.Service.Services;
using System.Text;

namespace MyDoctorAppointment
{
    public enum MainMenuOptions
    {
        Exit = 0,
        Doctors = 1,
        Patients = 2,
        Appointments = 3
    }

    public class Program
    {
        private static IDoctorService doctorService;
        private static IPatientService patientService;
        private static IAppointmentService appointmentService;

        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Оберіть формат збереження:");
            Console.WriteLine("1 - JSON");
            Console.WriteLine("2 - XML");
            Console.Write("Ваш вибір: ");
            string? formatChoice = Console.ReadLine();

            bool useJson = formatChoice == "1";

            var doctorStorage = useJson
                ? new DoctorRepository() as IDataStorage<Doctor>
                : new XmlRepository<Doctor>();

            var patientStorage = useJson
                ? new PatientRepository() as IDataStorage<Patient>
                : new XmlRepository<Patient>();

            var appointmentStorage = useJson
                ? new AppointmentRepository() as IDataStorage<Appointment>
                : new XmlRepository<Appointment>();

            doctorService = new DoctorService(doctorStorage);
            patientService = new PatientService(patientStorage);
            appointmentService = new AppointmentService(appointmentStorage);

            MainMenu();
        }

        private static void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Головне меню ---");
                Console.WriteLine("1) Робота з лікарями");
                Console.WriteLine("2) Робота з пацієнтами");
                Console.WriteLine("3) Робота з прийомами");
                Console.WriteLine("0) Вихід");
                Console.Write("Введіть свій вибір: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Невірне введення. Спробуйте знову.");
                    continue;
                }

                MainMenuOptions option = (MainMenuOptions)choice;
                switch (option)
                {
                    case MainMenuOptions.Doctors:
                        DoctorsMenu();
                        break;
                    case MainMenuOptions.Patients:
                        PatientsMenu();
                        break;
                    case MainMenuOptions.Appointments:
                        AppointmentsMenu();
                        break;
                    case MainMenuOptions.Exit:
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте знову.");
                        break;
                }
            }
        }

        #region Doctors

        private static void DoctorsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню лікарів ---");
                Console.WriteLine("1) Додати лікаря");
                Console.WriteLine("2) Показати всіх лікарів");
                Console.WriteLine("3) Оновити дані лікаря");
                Console.WriteLine("4) Видалити лікаря");
                Console.WriteLine("0) Назад");
                Console.Write("Введіть свій вибір: ");
                string input = Console.ReadLine();

                if (input == "0")
                    break;

                switch (input)
                {
                    case "1": AddDoctor(); break;
                    case "2": ShowAllDoctors(); break;
                    case "3": UpdateDoctor(); break;
                    case "4": DeleteDoctor(); break;
                    default: Console.WriteLine("Невірний вибір. Спробуйте знову."); break;
                }
            }
        }

        private static void AddDoctor()
        {
            Console.Write("Введіть ім'я лікаря: ");
            string name = Console.ReadLine();
            Console.Write("Введіть прізвище лікаря: ");
            string surname = Console.ReadLine();
            Console.Write("Введіть номер спеціалізації: ");
            int type = int.Parse(Console.ReadLine());

            Doctor doctor = new Doctor
            {
                Name = name,
                Surname = surname,
                DoctorType = (DoctorTypes)type,
                Experience = 0,
                Salary = 0
            };

            doctorService.Create(doctor);
            Console.WriteLine("Лікаря додано!");
        }

        private static void ShowAllDoctors()
        {
            var doctors = doctorService.GetAll();
            foreach (var d in doctors)
            {
                Console.WriteLine($"ID: {d.Id}, {d.Name} {d.Surname}, Спеціалізація: {d.DoctorType}");
            }
        }

        private static void UpdateDoctor()
        {
            Console.Write("Введіть ID лікаря для оновлення: ");
            int id = int.Parse(Console.ReadLine());
            var doctor = doctorService.Get(id);
            if (doctor == null)
            {
                Console.WriteLine("Лікаря не знайдено!");
                return;
            }

            Console.Write("Введіть нове ім'я (залиште порожнім, якщо не змінюється): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                doctor.Name = newName;

            doctorService.Update(id, doctor);
            Console.WriteLine("Дані лікаря оновлено!");
        }

        private static void DeleteDoctor()
        {
            Console.Write("Введіть ID лікаря для видалення: ");
            int id = int.Parse(Console.ReadLine());
            if (doctorService.Delete(id))
                Console.WriteLine("Лікаря видалено!");
            else
                Console.WriteLine("Лікаря не знайдено!");
        }

        #endregion

        #region Patients

        private static void PatientsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню пацієнтів ---");
                Console.WriteLine("1) Додати пацієнта");
                Console.WriteLine("2) Показати всіх пацієнтів");
                Console.WriteLine("3) Оновити дані пацієнта");
                Console.WriteLine("4) Видалити пацієнта");
                Console.WriteLine("0) Назад");
                Console.Write("Введіть свій вибір: ");
                string input = Console.ReadLine();

                if (input == "0")
                    break;

                switch (input)
                {
                    case "1": AddPatient(); break;
                    case "2": ShowAllPatients(); break;
                    case "3": UpdatePatient(); break;
                    case "4": DeletePatient(); break;
                    default: Console.WriteLine("Невірний вибір. Спробуйте знову."); break;
                }
            }
        }

        private static void AddPatient()
        {
            Console.Write("Введіть ім'я пацієнта: ");
            string name = Console.ReadLine();
            Console.Write("Введіть прізвище пацієнта: ");
            string surname = Console.ReadLine();
            Console.Write("Введіть номер типу захворювання (відповідний IllnessTypes): ");
            int illnessType = int.Parse(Console.ReadLine());
            Console.Write("Введіть адресу пацієнта: ");
            string address = Console.ReadLine();

            Patient patient = new Patient
            {
                Name = name,
                Surname = surname,
                IllnessType = (IllnessTypes)illnessType,
                Address = address
            };

            patientService.Create(patient);
            Console.WriteLine("Пацієнта додано!");
        }

        private static void ShowAllPatients()
        {
            var patients = patientService.GetAll();
            foreach (var p in patients)
            {
                Console.WriteLine($"ID: {p.Id}, {p.Name} {p.Surname}, Захворювання: {p.IllnessType}, Адреса: {p.Address}");
            }
        }

        private static void UpdatePatient()
        {
            Console.Write("Введіть ID пацієнта для оновлення: ");
            int id = int.Parse(Console.ReadLine());
            var patient = patientService.Get(id);
            if (patient == null)
            {
                Console.WriteLine("Пацієнта не знайдено!");
                return;
            }

            Console.Write("Введіть нове ім'я (залиште порожнім, якщо не змінюється): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                patient.Name = newName;

            patientService.Update(id, patient);
            Console.WriteLine("Дані пацієнта оновлено!");
        }

        private static void DeletePatient()
        {
            Console.Write("Введіть ID пацієнта для видалення: ");
            int id = int.Parse(Console.ReadLine());
            if (patientService.Delete(id))
                Console.WriteLine("Пацієнта видалено!");
            else
                Console.WriteLine("Пацієнта не знайдено!");
        }

        #endregion

        #region Appointments

        private static void AppointmentsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню прийомів ---");
                Console.WriteLine("1) Додати прийом");
                Console.WriteLine("2) Показати всі прийоми");
                Console.WriteLine("3) Оновити прийом");
                Console.WriteLine("4) Видалити прийом");
                Console.WriteLine("0) Назад");
                Console.Write("Введіть свій вибір: ");
                string input = Console.ReadLine();

                if (input == "0")
                    break;

                switch (input)
                {
                    case "1": AddAppointment(); break;
                    case "2": ShowAllAppointments(); break;
                    case "3": UpdateAppointment(); break;
                    case "4": DeleteAppointment(); break;
                    default: Console.WriteLine("Невірний вибір. Спробуйте знову."); break;
                }
            }
        }

        private static void AddAppointment()
        {
            Console.Write("Введіть ID лікаря: ");
            int doctorId = int.Parse(Console.ReadLine());
            Console.Write("Введіть ID пацієнта: ");
            int patientId = int.Parse(Console.ReadLine());
            Console.Write("Введіть дату та час початку прийому (наприклад, 2025-03-22 10:00): ");
            DateTime start = DateTime.Parse(Console.ReadLine());
            Console.Write("Введіть дату та час закінчення прийому (наприклад, 2025-03-22 11:00): ");
            DateTime end = DateTime.Parse(Console.ReadLine());
            Console.Write("Введіть опис прийому: ");
            string description = Console.ReadLine();

            Doctor doctor = doctorService.Get(doctorId);
            Patient patient = patientService.Get(patientId);

            Appointment appointment = new Appointment
            {
                Doctor = doctor,
                Patient = patient,
                DateTimeFrom = start,
                DateTimeTo = end,
                Description = description
            };

            appointmentService.Create(appointment);
            Console.WriteLine("Прийом додано!");
        }

        private static void ShowAllAppointments()
        {
            var appointments = appointmentService.GetAll();
            foreach (var a in appointments)
            {
                Console.WriteLine($"ID: {a.Id}, Лікар: {a.Doctor?.Name}, Пацієнт: {a.Patient?.Name}, Початок: {a.DateTimeFrom}, Кінець: {a.DateTimeTo}, Опис: {a.Description}");
            }
        }

        private static void UpdateAppointment()
        {
            Console.Write("Введіть ID прийому для оновлення: ");
            int id = int.Parse(Console.ReadLine());
            var appointment = appointmentService.Get(id);
            if (appointment == null)
            {
                Console.WriteLine("Прийом не знайдено!");
                return;
            }

            Console.Write("Новий ID лікаря (або Enter): ");
            string doctorInput = Console.ReadLine();
            if (int.TryParse(doctorInput, out int newDoctorId))
                appointment.Doctor = doctorService.Get(newDoctorId);

            Console.Write("Новий ID пацієнта (або Enter): ");
            string patientInput = Console.ReadLine();
            if (int.TryParse(patientInput, out int newPatientId))
                appointment.Patient = patientService.Get(newPatientId);

            Console.Write("Нова дата початку (або Enter): ");
            string startInput = Console.ReadLine();
            if (DateTime.TryParse(startInput, out DateTime newStart))
                appointment.DateTimeFrom = newStart;

            Console.Write("Нова дата завершення (або Enter): ");
            string endInput = Console.ReadLine();
            if (DateTime.TryParse(endInput, out DateTime newEnd))
                appointment.DateTimeTo = newEnd;

            Console.Write("Новий опис (або Enter): ");
            string newDesc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDesc))
                appointment.Description = newDesc;

            appointmentService.Update(id, appointment);
            Console.WriteLine("Прийом оновлено!");
        }

        private static void DeleteAppointment()
        {
            Console.Write("Введіть ID прийому для видалення: ");
            int id = int.Parse(Console.ReadLine());
            if (appointmentService.Delete(id))
                Console.WriteLine("Прийом видалено!");
            else
                Console.WriteLine("Прийом не знайдено!");
        }

        #endregion
    }
}
