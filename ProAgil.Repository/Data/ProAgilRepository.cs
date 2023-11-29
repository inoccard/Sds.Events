using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Entities;

namespace ProAgil.Repository.Data
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext context;

        public ProAgilRepository(ProAgilContext context)
        {
            this.context = context;
        }

        #region GERAIS

        public void Add<T>(T entity) where T : class => context.Add(entity);

        public void Update<T>(T entity) where T : class => context.Update(entity);

        public void Delete<T>(T entity) where T : class => context.Remove(entity);

        public void DeleteRange<T>(T[] entity) where T : class => context.RemoveRange(entity);

        public async Task<bool> SaveChangeAssync() => await context.SaveChangesAsync() > 0;

        #endregion GERAIS

        #region Consulta

        public async Task<Event> GetEventAssyncById(int Id, bool includeSpeaker = false)
        {
            IQueryable<Event> events = context.Events
                 .Include(e => e.Lots)
                 .Include(e => e.SocialNetworks);

            if (includeSpeaker)
            {
                events = events.Include(se => se.SpeakerEvents)
                    .ThenInclude(s => s.Spreaker);
            }

            events = events.OrderByDescending(e => e.EventDate).Where(e => e.Id == Id);

            return await events.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Event[]> GetEventsAssync(bool includeSpeaker)
        {
            IQueryable<Event> events = context.Events
               .Include(e => e.Lots)
               .Include(e => e.SocialNetworks);

            if (includeSpeaker)
            {
                events = events.Include(se => se.SpeakerEvents)
                    .ThenInclude(s => s.Spreaker);
            }

            events = events.AsNoTracking().OrderByDescending(e => e.EventDate);

            return await events.ToArrayAsync();
        }

        public async Task<Event[]> GetEventsAssyncByTheme(string theme, bool includeSpeaker)
        {
            IQueryable<Event> events = context.Events
                .Include(e => e.Lots) // inclui os lotes
                .Include(e => e.SocialNetworks); // e as redes sociais

            // referência muitos para muitos (n:n)
            if (includeSpeaker)
            {
                events = events.Include(se => se.SpeakerEvents)
                    .ThenInclude(s => s.Spreaker);
            }

            events = events.OrderByDescending(e => e.EventDate).Where(e => e.Theme.ToLower().Contains(theme));

            return await events.AsNoTracking().ToArrayAsync();
        }

        public async Task<Speaker[]> GetSpeakersAssync(bool includeEvents = false)
        {
            IQueryable<Speaker> speakers = context.Speakers
                 .Include(e => e.SocialNetworks);

            if (includeEvents)
            {
                speakers = speakers.Include(se => se.SpeakerEvents)
                    .ThenInclude(s => s.Event);
            }

            speakers = speakers.OrderBy(e => e.User.FullName);

            return await speakers.AsNoTracking().ToArrayAsync();
        }

        public async Task<Speaker> GetSpeakersAssyncById(int Id, bool includeEvents)
        {
            IQueryable<Speaker> speakers = context.Speakers
               .Include(e => e.SocialNetworks);

            if (includeEvents)
            {
                speakers = speakers.Include(se => se.SpeakerEvents)
                    .ThenInclude(s => s.Event);
            }

            speakers = speakers.Where(e => e.Id == Id).OrderBy(e => e.User.FullName);

            return await speakers.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Speaker[]> GetSpeakersAssyncByName(string name, bool includeEvents)
        {
            IQueryable<Speaker> speakers = context.Speakers
               .Include(e => e.SocialNetworks);

            if (includeEvents)
            {
                speakers = speakers.Include(se => se.SpeakerEvents)
                    .ThenInclude(s => s.Event);
            }

            speakers = speakers.Where(e => e.User.FullName.ToLower().Contains(name));

            return await speakers.AsNoTracking().ToArrayAsync();
        }

        #endregion Consulta
    }
}