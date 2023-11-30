using Sds.Events.Domain.Entities;
using System.Threading.Tasks;

namespace Sds.Events.Repository.Data
{
    public interface IEventsRepository
    {
        /// <summary>
        /// Tipo Generico
        /// Adicionar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Add<T>(T entity) where T : class;
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Update<T>(T entity) where T : class;
        /// <summary>
        /// Excluir
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Excluir vários objetos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void DeleteRange<T>(T[] entity) where T : class;

        Task<bool> SaveChangeAssync();

        /// <summary>
        /// Obtém Eventos por tema
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        Task<Event[]> GetEventsAssyncByTheme(string theme, bool includeSpeaker);

        /// <summary>
        /// Obtém Todos os eventos
        /// </summary>
        /// <param name="includeSpeaker"></param>
        /// <returns></returns>
        Task<Event[]> GetEventsAssync(bool includeSpeaker);

        /// <summary>
        /// Obtém eventos por ID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="includeSpeaker"></param>
        /// <returns></returns>
        Task<Event> GetEventAssyncById(int Id, bool includeSpeaker = false);

        /// <summary>
        /// Obtém palestrantes
        /// </summary>
        /// <param name="includeEvents"></param>
        /// <returns></returns>
        Task<Speaker[]> GetSpeakersAssync(bool includeEvents = false);

        /// <summary>
        /// Obtém palestrantes por ID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="includeEvents"></param>
        /// <returns></returns>
        Task<Speaker> GetSpeakersAssyncById(int Id, bool includeEvents = false);

        /// <summary>
        /// Obtém Eventos por tema
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Speaker[]> GetSpeakersAssyncByName(string name, bool includeEvents = false);
    }
}