using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace Simple_Helpdesk.Models
{
    /// <summary>
    /// Класс контекста данных для Entity Framework
    /// </summary>
    public class MyRequestContext : DbContext
    {
        public MyRequestContext(string connectionStringName)
            : base(connectionStringName) {
            // Если база не найдена, то создадим её и инициализируем несколькими значениями
            Database.SetInitializer<MyRequestContext>(new SampleInitializer());
        }

        // отображения нижеуказанных классов в базу данных 
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestDescription> Descriptions { get; set; }
    }

    // обрабатываем ситуацию, когда база создается с нуля
    public class SampleInitializer : CreateDatabaseIfNotExists<MyRequestContext>
    {
        protected override void Seed(MyRequestContext context) {
            // первая заявка, 3 изменения состояния
            var request = makeRequest("Заявка на трубы");
            request.Descriptions.Add(makeStatus(RequestStatus.Opened, "18/12/2017 09:00", "Закупить трубы \"такие-то\""));
            request.Descriptions.Add(makeStatus(RequestStatus.Solved, "20/12/2017 09:00", "Закуплены"));
            request.Descriptions.Add(makeStatus(RequestStatus.Closed, "21/12/2017 09:00", "Проверено, закрыто как исполненная"));
            context.Requests.Add(request);

            // вторая заявка, 4 состояние
            request = makeRequest("Протекает на Водосточной");
            request.Descriptions.Add(makeStatus(RequestStatus.Opened, "19/12/2017 09:00", "Нужно решить проблему"));
            request.Descriptions.Add(makeStatus(RequestStatus.Solved, "21/12/2017 09:00", "Назначили Михалыча"));
            request.Descriptions.Add(makeStatus(RequestStatus.Returned, "22/12/2017 09:00", "Михалыч в отпуске, отправьте еще кого-нить"));
            request.Descriptions.Add(makeStatus(RequestStatus.Solved, "23/12/2017 09:00", "Был не отмечен в графике, отправили Васильевича"));
            context.Requests.Add(request);

            // третья заявка, 1 состояние
            request = makeRequest("Рассылка сломалась");
            request.Descriptions.Add(makeStatus(RequestStatus.Opened, "20/12/2017 09:00", "Акты выполненных работ не рассылаются"));
            context.Requests.Add(request);

            // четвертая, 3 состояния
            request = makeRequest("Проблемы с принтером");
            request.Descriptions.Add(makeStatus(RequestStatus.Opened, "21/12/2017 09:00", "Не печатает"));
            request.Descriptions.Add(makeStatus(RequestStatus.Solved, "21/12/2017 13:00", "Тонер закончился, заправили, проверяйте"));
            request.Descriptions.Add(makeStatus(RequestStatus.Closed, "21/12/2017 15:00", "Печатает, спасибо"));
            context.Requests.Add(request);

            context.SaveChanges();
            
            /* // Entity Framework сгенерирует ниже описаный T-SQL код для создания таблиц
            
            context.Database.ExecuteSqlCommand(
                    @"CREATE TABLE [dbo].[Requests] (
                    [ID]   INT           IDENTITY (1, 1) NOT NULL,
                    [Name] NVARCHAR (30) NOT NULL,
                    CONSTRAINT [PK_dbo.Requests] PRIMARY KEY CLUSTERED ([ID] ASC));"
                    );

            context.Database.ExecuteSqlCommand(
                    @"CREATE TABLE [dbo].[RequestDescriptions] (
                    [ID]               INT            IDENTITY (1, 1) NOT NULL,
                    [RequestID]        INT            NOT NULL,
                    [Status]           INT            NOT NULL,
                    [ModificationTime] DATETIME       NULL,
                    [Description]      NVARCHAR (200) NOT NULL,
                    CONSTRAINT [PK_dbo.RequestDescriptions] PRIMARY KEY CLUSTERED ([ID] ASC),
                    CONSTRAINT [FK_dbo.RequestDescriptions_dbo.Requests_RequestID] FOREIGN KEY ([RequestID]) REFERENCES [dbo].[Requests] ([ID]) ON DELETE CASCADE);"
                    );
            */

            base.Seed(context);
        }

        private Request makeRequest(string name) {
            return new Request() {
                Name = name,
                Descriptions = new List<RequestDescription>()
            };
        }

        /// <param name="modifDateTime">"dd/MM/yyyy HH:mm"</param>
        private RequestDescription makeStatus(RequestStatus status, string modifDateTime, string description) {
            return new RequestDescription() {
                Status = status,
                ModificationTime = DateTime.ParseExact(modifDateTime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                Description = description
            };
        }
    }
}